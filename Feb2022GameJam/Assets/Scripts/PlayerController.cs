using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRidge;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerRidge.velocity = playerInput * 7;
    }
}
