using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRidge;
    public int playerNumber;
    GameController gameController;
    //bool canMove;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        //canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D col)  {
        if (col.collider.gameObject.tag == "Asteroid") {
           Destroy(col.collider.gameObject);
        }

        
    }

    void FixedUpdate()
    {
        if (gameController.MoveCheck()) {
            Vector2 playerInput = new Vector2(0, 0);
            if (playerNumber == 1)
            {
                playerInput = new Vector2(Input.GetAxis("Horizontal 1"), Input.GetAxis("Vertical 1"));
            }
            else if (playerNumber == 2)
            {
                playerInput = new Vector2(Input.GetAxis("Horizontal 2"), Input.GetAxis("Vertical 2"));
            }
            playerRidge.velocity = playerInput * 20;
        } else {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
