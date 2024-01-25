using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;
    void Start()
    {
        player = FindObjectOfType<MovePlayer>().gameObject;
    }

    //Moves the car when the player is close enough to it
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position,player.transform.position) < 50)
        {
            transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,transform.position.z- speed * Time.deltaTime);
        }
    }
}
