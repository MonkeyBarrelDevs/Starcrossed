using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDespawnManager : MonoBehaviour
{
    AsteroidManager asteroidManager;
    private float y = 0;
    private float randomSpinMultiplier;
    private double spinTimer;
    // Start is called before the first frame update
    void Start()
    {
        randomSpinMultiplier = Random.Range(1, 5);
        asteroidManager = FindObjectOfType<AsteroidManager>();
    }

    void spinAsteroid() {
        y += 0.5f * randomSpinMultiplier * Time.timeScale;
        transform.localRotation = Quaternion.Euler(0, 0, y);

        
    }

    // Update is called once per frame
    void Update()
    {
        spinAsteroid();
        //y += 1 * randomSpinMultiplier;
        //Debug.Log("X: " + x + "Y: " + y);
        if (transform.position.magnitude > asteroidManager.GetDestroyRadius()) {
            Destroy(this.gameObject);
        }
        


    }
}
