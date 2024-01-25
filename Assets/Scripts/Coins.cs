using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public MovePlayer player;
    public GameObject playerReference;
    public int value = 1;

    void Start()
    {
        player = playerReference.GetComponent<MovePlayer>();
    }
    void OnTriggerEnter(Collider other)
    {
        player = other.gameObject.GetComponent<MovePlayer>();

        if (other.tag == "Player")
        {
            player.audioSource.clip = player.coinPickUp;
            player.audioSource.Play();
            player.displayCoins += value;
            player.coins += value;
            Destroy(gameObject);
        }
    }




}
