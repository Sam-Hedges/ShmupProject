using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Nitro : MonoBehaviour
{
    // A script that provides a boost to the player's speed when they collide with a Nitro Canister.
    MovePlayer player; // A reference to the player object's MovePlayer script.
    public GameObject playerReference; // A reference to the player object.
    float speedBoost = 10; // The amount to boost the player's speed by.
    float currentSpeed; // The player's current speed.
    int maxSpeed = 80;

    void Start()
    {
        // Get a reference to the player object's MovePlayer script.
        player = playerReference.GetComponent<MovePlayer>();
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the trigger is the player.
        if (other.tag == "Player")
        {
            // Get a reference to the player object's MovePlayer script.
            player = other.gameObject.GetComponent<MovePlayer>();

            // Call the NitroBoost method to boost the player's speed.
            NitroBoost(player);
        }
    }

    // A method that boosts the player's speed.
    public void NitroBoost(MovePlayer player)
    {
        // Check if the player's nitro is not already active.
        if (player.speedActive != true)
        {
            // Boost the player's speed.
            player.currentSpeed = player.speed;
            player.speed += speedBoost;
            if (player.speed > player.maxSpeed)
            {
                player.speed = maxSpeed;
            }
            player.speedActive = true;
            player.duration = player.maxDuration;
        }
    }
}

