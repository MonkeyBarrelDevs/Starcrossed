using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour
{
    public GameController controller;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.tag == "Player") {
            FindObjectOfType<AudioManager>().Play("blackHoleEarthImpact");
            controller.FailGame();
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        } else if (col.collider.gameObject.tag == "Asteroid") {
            FindObjectOfType<AudioManager>().Play("meteorEarthImpact");
            controller.FailGame();
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }

        
    }


}
