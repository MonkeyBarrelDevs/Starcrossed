using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour
{
    public GameController controller;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.tag == "Player" || col.collider.gameObject.tag == "Asteroid")
        {
            Debug.Log("hi!");
            controller.FailGame();
        }

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }


}
