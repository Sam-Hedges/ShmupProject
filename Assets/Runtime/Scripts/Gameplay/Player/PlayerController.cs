using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : StateManager<EPlayerState>, IDamageable
{
    [Header("References")]
    public InputHandler inputHandler;
    public BulletPoolManager bulletPoolManager;
    internal CharacterController characterController;

    [Header("Player Stats")]
    [SerializeField]
    private int maximumLives = 5;
    private int _currentLives;

    private Coroutine _attackCoroutine;
    [Header("Shooting")]
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private Transform meshContainerTransform;
    [SerializeField]
    internal float fireRate = 0.1f;
    [SerializeField]
    internal float bulletSpeed = 200f;
    [SerializeField]
    internal float bulletLifetime = 5f;

    private AbilityState _abilityState = AbilityState.Ready;
    private Coroutine _abilityCoroutine;
    [Header("Abilities")]
    [SerializeField]
    private List<Ability> abilities;

    [SerializeField]
    private float abilityCooldownTime;
    [SerializeField]
    private float abilityActiveTime;

    [Header("Movement")]
    [SerializeField]
    internal float dashDuration = 0.5f;
    [SerializeField]
    internal float dashCooldownSeconds = 1f;
    [SerializeField]
    private float tiltAngle = 30f; // hard limit for tilt angle
    [SerializeField]
    private float tiltSpeed = 15f; // how much tilt changes with velocity
    [SerializeField]
    private float minThrottleAngle = 15f; // hard limit for tilt angle
    [SerializeField]
    private float maxThrottleAngle = 15f; // hard limit for tilt angle
    [SerializeField]
    private float throttleSpeed = 2f; // how much tilt changes with velocity
    [SerializeField]
    private List<EngineTransforms>
        engineTransforms; // array of arrays of engine transforms

    [System.Serializable]
    public class EngineTransforms
    {
        public Transform topFlap;
        public Transform leftFlap;
        public Transform rightFlap;
    }

    internal Vector3 velocity;
    internal Vector3 lastPosition;
    internal Vector2 movementInputVector;
    internal bool isMovementPressed;
    internal bool isDashing;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        // Instantiate the states and populate the States dictionary.
        States[EPlayerState.Idle] = new Idle(this);
        States[EPlayerState.Moving] = new Moving(this);
        States[EPlayerState.Dashing] = new Dashing(this);

        CurrentState = States[EPlayerState.Idle];

        _currentLives = maximumLives;
    }

    // Adds listeners for events being triggered in the InputReader script
    private void OnEnable()
    {
        inputHandler.EnableGameplayInput();
        inputHandler.DodgeEvent += OnDashInput;
        inputHandler.MoveEvent += OnMovementInput;
        inputHandler.AttackEvent += OnAttackInput;
        inputHandler.AttackCanceledEvent += OnAttackCanceledInput;
        inputHandler.SkillEvent_01 += OnSkillInput_01;
        //...
    }

    // Removes all listeners to the events coming from the InputReader script
    private void OnDisable()
    {
        inputHandler.DisableAllInput();
        inputHandler.DodgeEvent -= OnDashInput;
        inputHandler.MoveEvent -= OnMovementInput;
        inputHandler.AttackEvent -= OnAttackInput;
        inputHandler.AttackCanceledEvent -= OnAttackCanceledInput;
        inputHandler.SkillEvent_01 -= OnSkillInput_01;
        //...
    }

    internal void OnMovementInput(Vector2 input)
    {
        // Create new vector2 to avoid yielding weird results from the callback
        // context
        Vector2 inputValue = new Vector2(input.x, input.y);

        movementInputVector = inputValue;
        isMovementPressed = movementInputVector != Vector2.zero;
    }

    private void OnSkillInput_01()
    {
        if (_abilityState != AbilityState.Ready)
            return;
        _abilityCoroutine = StartCoroutine(TriggerAbility());
    }

    private IEnumerator TriggerAbility(int listIndex = 0)
    {
        abilities[listIndex].Activate(gameObject);
        _abilityState = AbilityState.Active;
        abilityActiveTime = abilities[listIndex].activeTime;

        while (_abilityState == AbilityState.Active)
        {
            if (abilityActiveTime > 0)
            {
                abilityActiveTime -= Time.fixedDeltaTime;
            }
            else
            {
                abilities[listIndex].BeginCooldown(gameObject);
                _abilityState = AbilityState.Cooldown;
                abilityCooldownTime = abilities[listIndex].cooldownTime;
            }
            yield return new WaitForFixedUpdate();
        }

        while (_abilityState == AbilityState.Cooldown)
        {
            if (abilityCooldownTime > 0)
            {
                abilityCooldownTime -= Time.fixedDeltaTime;
            }
            else
            {
                _abilityState = AbilityState.Ready;
            }
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    internal void OnDashInput()
    {

        if (!isDashing && isMovementPressed)
        {
            // isDashing = true;
        }
    }

    // Legacy input system
    internal void OnAttackInput(InputAction.CallbackContext context)
    {

        Vector3 direction = transform.forward;
        Vector3 spawnPosition = bulletSpawnPoint.position;
        Bullet bullet = bulletPoolManager.RequestBullet();
        bullet.transform.position = spawnPosition;
        bullet.Fire(direction, 50f, bulletPoolManager);
        StartCoroutine(BulletLifetime(bullet));
    }

    internal void OnAttackInput() { _attackCoroutine = StartCoroutine(Fire()); }

    internal void OnAttackCanceledInput() { StopCoroutine(_attackCoroutine); }

    private IEnumerator Fire()
    {
        while (true) // The coroutine gets cancelled on input release
        {
            Vector3 direction = transform.forward;
            Vector3 spawnPosition = bulletSpawnPoint.position;
            Bullet bullet = bulletPoolManager.RequestBullet();
            bullet.transform.position = spawnPosition;
            bullet.Fire(direction, bulletSpeed, bulletPoolManager);
            StartCoroutine(BulletLifetime(bullet));
            yield return new WaitForSeconds(fireRate);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private IEnumerator BulletLifetime(Bullet bullet)
    {
        yield return new WaitForSeconds(bulletLifetime);
        bulletPoolManager.DisableBullet(bullet);
    }

    private void FixedUpdate()
    {
        Velocity();
        TiltPlayer();
        ThrottleEngines();
    }

    private void Velocity()
    {

        Vector3 position = gameObject.transform.position;
        velocity = (position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = position;
    }

    private void TiltPlayer()
    {
        float tilt = Mathf.Clamp(velocity.x, -tiltAngle,
                                 tiltAngle); // calculate the tilt angle
        meshContainerTransform.rotation = Quaternion.Slerp(
            meshContainerTransform.rotation, Quaternion.Euler(0, 0, -tilt),
            Time.fixedDeltaTime *
                tiltSpeed); // calculate and assign the quaternion, tilting over
                            // the X axis
    }

    private void ThrottleEngines()
    {
        float tilt = Mathf.Clamp(velocity.z, -maxThrottleAngle,
                                 minThrottleAngle); // calculate the tilt angle

        foreach (var engine in engineTransforms)
        {
            engine.topFlap.localRotation = Quaternion.Slerp(
                engine.topFlap.localRotation, Quaternion.Euler(-tilt, 0, 0),
                Time.fixedDeltaTime * throttleSpeed);
            engine.rightFlap.localRotation = Quaternion.Slerp(
                engine.rightFlap.localRotation, Quaternion.Euler(-tilt, 120, 0),
                Time.fixedDeltaTime * throttleSpeed);
            engine.leftFlap.localRotation = Quaternion.Slerp(
                engine.leftFlap.localRotation, Quaternion.Euler(-tilt, -120, 0),
                Time.fixedDeltaTime * throttleSpeed);
        }
    }

    internal void Move(Vector2 inputVector)
    {
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        // Assuming moveSpeed is a field that defines how fast your character
        // should move
        float moveSpeed = 100f; // Set this to your desired speed

        // Apply the movement speed and time factor
        moveDirection *= moveSpeed * Time.deltaTime;

        characterController.Move(moveDirection);
    }

    // To support external coroutines being called:
    internal void StartChildCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }
    internal void StopChildCoroutine(IEnumerator coroutineMethod)
    {
        StopCoroutine(coroutineMethod);
    }

    public void Damage(float amount) { _currentLives -= 1; }
}
