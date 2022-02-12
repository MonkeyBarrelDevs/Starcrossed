using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDespawnManager : MonoBehaviour
{
    AsteroidManager asteroidManager;
    // Start is called before the first frame update
    void Start()
    {
        asteroidManager = FindObjectOfType<AsteroidManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > asteroidManager.GetDestroyRadius()) {
            Destroy(this.gameObject);
        }
    }
}
