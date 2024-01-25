using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class MovePlayer : MonoBehaviour
{
    //Calls the character controller functionality
    CharacterController characterController;

    //contains all variables for player movement
    #region playerVariables
    public float speed;
    public float currentSpeed;
    public int skinNum;
    [SerializeField]
    int movePlayer = 3;
    public int maxSpeed = 40;
    [SerializeField]
    float dodgeSpeed = 0.3f;
    public int playerPos = 1;
    #endregion

    #region skins
    public Material[] defaultSkin;
    public Material[] transparentSkin;

    #endregion
    private Vector3 startingPos, currentPos;
    public int distance;
    //Array for the lanes the player can move between
    [SerializeField]
    float[] movePositions = new float[3];
    [SerializeField]
    Animator ship;

    public MeshRenderer[] mesh;
    public Light shipLight, shipLightDamage;

    public VisualEffect sparks;

    public AudioClip coinPickUp, hurtSound;
    public AudioSource audioSource;

    public int coins, displayCoins;
    int value = 1;

    public int health;
    public int maxHealth;
    public bool beenHit, hitInvinc, speedActive;
    public float duration, maxDuration, speedGain;
    public GameObject shield;
    public CinemachineVirtualCamera mainCamera;
    float healTime;

    bool moveInProgress;

    public bool motionBlurOn;
    public LevelGeneration levelGenerator;
    public OptionsMenu options;

    // Start is called before the first frame update
    void Start()
    {

        shield.SetActive(false);
        sparks.Stop();
        Time.timeScale = 1f;
        currentSpeed = 20f;
        speedActive = true;
        speedGain = 0.0003f;
        options.OptionsLoad();
        options.ChangeAA(options.AAmode);
        SaveSystem.LoadPlayer(this);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        startingPos = transform.position;
        skinNum = SaveSystem.LoadData("skin");
        displayCoins = SaveSystem.LoadData("coins");
        coins = 0;
        shipLightDamage.enabled = false;
        ship.speed = (1 - dodgeSpeed) + 1;
        Debug.Log("Start finished");
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(0, 0, speed);
        currentPos = transform.position;
        if (moveInProgress == false)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (playerPos < 2)
                {
                    playerPos++;
                    StartCoroutine(LerpPosition(new Vector3(movePositions[playerPos], transform.position.y, transform.position.z + speed * dodgeSpeed), dodgeSpeed, "TurnRight"));
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (playerPos > 0)
                {
                    playerPos--;
                    StartCoroutine(LerpPosition(new Vector3(movePositions[playerPos], transform.position.y, transform.position.z + speed * dodgeSpeed), dodgeSpeed, "TurnLeft"));
                }
            }
        }



        characterController.Move(move * Time.deltaTime); 
        distance = (int)(currentPos - startingPos).magnitude;
        if (Time.timeScale != 0 && beenHit == false && speed != maxSpeed && speedActive == false)
        {
            speed += speedGain* levelGenerator.difficulty;
        }
        if (speedActive == true)
        {
            shield.SetActive(true);
            StartCoroutine(ChangeFOV(mainCamera, 110, 0.5f));
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                StartCoroutine(ChangeFOV(mainCamera, 100, 0.5f));
                shield.SetActive(false);
                duration = maxDuration;
                speedActive = false;
                speed = currentSpeed;
                speedActive = false;
            }
        }

        if (beenHit == true && hitInvinc == false)
        {
            healTime -= Time.deltaTime;
            if (healTime <= 0)
            {
                shipLightDamage.enabled = false;
                shipLight.enabled = true;
                sparks.Stop();
                health = maxHealth;
                beenHit = false;
                healTime = 3f;
            }
        }


    }
    IEnumerator ChangeFOV(CinemachineVirtualCamera cam, float endFOV, float duration)
    {
        float startFOV = cam.m_Lens.FieldOfView;
        float time = 0;
        while (time < duration)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, endFOV, time / duration);
            yield return null;
            time += Time.deltaTime;
        }
    }
    // A coroutine function that smoothly interpolates the position of a game object from its current position to a target position over a specified duration, while triggering an animation during the movement.
    IEnumerator LerpPosition(Vector3 targetPosition, float duration, string animation)
    {
        // Set moveInProgress flag to true to indicate that the object is currently moving.
        moveInProgress = true;

        // Trigger the specified animation on the ship object.
        ship.SetTrigger(animation);

        // Initialize time to 0 and startPosition to the current position of the object.
        float time = 0f;
        Vector3 startPosition = transform.position;

        // Loop until the time elapsed since the start of the interpolation reaches the specified duration plus a buffer of 0.03 seconds.
        while (time < duration + 0.03)
        {
            // Interpolate the position of the object using the Lerp method and update the object's position.
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            // Increment the time elapsed since the start of the interpolation by the time since the last frame.
            time += Time.deltaTime;

            // Yield control back to the coroutine scheduler to allow other coroutines to run in the meantime.
            yield return null;
        }

        // Once the interpolation is complete, set the object's position to the target position and set moveInProgress flag to false to indicate that the object has finished moving.
        transform.position = targetPosition;
        moveInProgress = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && speedActive == false && hitInvinc == false)
        {
            health--;
            if (health <= -1)
            {
                audioSource.clip = hurtSound;
                audioSource.Play();
                SaveGame();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                VariableTransfer.currentDistance = distance;
                SceneManager.LoadScene(2);
            }
            else
            {
                hitInvinc = true;
                beenHit = true;
                StartCoroutine(DamageSpeed());
            }
        }

    }

    // A coroutine that handles the ship taking damage and slowing down temporarily.
    IEnumerator DamageSpeed()
    {
        // Enable the light damage effect and disable the normal ship light.
        shipLightDamage.enabled = true;
        shipLight.enabled = false;

        // Play the sparks particle effect and the hurt sound.
        sparks.Play();
        audioSource.clip = hurtSound;
        audioSource.Play();

        // Trigger the TakesDamage animation on the ship.
        ship.SetTrigger("TakesDamage");

        // Halve the ship's speed and calculate the speed difference.
        currentSpeed = speed;
        speed /= 2;
        float speedDifference = speed / 2;

        // If the ship's speed is below a certain threshold, set it to the minimum speed.
        if (speed < 20f)
        {
            speed = 20f;
        }

        // Wait for 3 seconds before continuing.
        yield return new WaitForSeconds(1f);
        hitInvinc = false;
        //yield return new WaitForSeconds(2f);
        //// Set the "been hit" flag to true and restore the ship's speed.
        //beenHit = false;
        //speed += speedDifference;
    }

    void CheckSpeed()
    {
        if (distance > 0f)
        {
            if (speed < maxSpeed)
            {
                float multiplier = (1f + levelGenerator.difficulty) * Time.deltaTime;
                speed += multiplier;
            }


        }
    }
    public void SaveGame()
    {
        VariableTransfer.distance = distance;
        SaveSystem.CompareDistance(distance, SaveSystem.LoadData("distance"));
        SaveSystem.AddCoins(coins, SaveSystem.LoadData("coins"));
    }
}