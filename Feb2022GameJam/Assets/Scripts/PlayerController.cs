using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRidge;
    public int playerNumber;

    void Start()
    {
        
    }

    void FixedUpdate()
    {

        Vector2 playerInput = new Vector2(0, 0);
        if (playerNumber == 1)
        {
            playerInput = new Vector2(Input.GetAxis("Horizontal 1"), Input.GetAxis("Vertical 1"));
        }
        else if (playerNumber == 2)
        {
            playerInput = new Vector2(Input.GetAxis("Horizontal 2"), Input.GetAxis("Vertical 2"));
        }
        playerRidge.velocity = playerInput * 7;
    }
}
