using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerL : MonoBehaviour
{
    private CharacterController characterController;
    
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float tiltAngle = 30f; // hard limit for tilt angle
    [SerializeField] private float tiltSpeed = 15f; // how much tilt changes with velocity
    
    private Vector3 velocity;
    private Vector3 lastPosition;
    private Vector2 movementInputVector;
    private bool isMovementPressed;
    private bool isDodging;
    

    // Start is called before the first frame update
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        movementInputVector = context.ReadValue<Vector2>();
        isMovementPressed = movementInputVector != Vector2.zero;
    }
    
    public void OnDodgeInput(InputAction.CallbackContext context) {

        if (context.ReadValue<float>() > 0 && !isDodging && isMovementPressed) {
            isDodging = true;
            StartCoroutine(HandleDodge());
        }
    }

    private void Velocity() {
        
        Vector3 position = gameObject.transform.position;
        velocity = (position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = position;
    }

    private void TiltPlayer()
    {
        float tilt = Mathf.Clamp(velocity.x, -tiltAngle , tiltAngle ); // calculate the tilt angle
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -tilt), Time.fixedDeltaTime * tiltSpeed); // calculate and assign the quaternion, tilting over the X axis
    }
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        Velocity();
        TiltPlayer();
        HandleMovement(movementInputVector);
    }
    
    private void HandleMovement(Vector2 inputVector)
    {

        if (!isMovementPressed && !isDodging) { return; }
        
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        
        characterController.Move(moveDirection);

    }
    
    IEnumerator HandleDodge()
    {
        Vector2 lastMoveDirection = movementInputVector;
        
        // Gets the starting time of the dash
        float startTime = Time.time;

        // While the current time is smaller than the (start time + the dash time) then run the code within the while loop
        while (Time.time < startTime + dashTime)
        {
            /* Moves the character in the last faced direction and tells the Move() method that this
            movement is currently a dash so it uses the dash speed rather than the default speed */
            HandleMovement(lastMoveDirection);

            // Forces the coroutine to wait until the next frame until it can run again. This makes the while loop act as a update while active
            yield return null;
        }

        // Tells the rest of the script that the dash has finished
        isDodging = false;

        // Ends the coroutine
        yield break;
    }
    
}