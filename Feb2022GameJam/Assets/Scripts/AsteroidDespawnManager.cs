using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDespawnManager : MonoBehaviour
{
    AsteroidManager asteroidManager;
    private int x = 0;
    private int y = 0;
    // Start is called before the first frame update
    void Start()
    {
        asteroidManager = FindObjectOfType<AsteroidManager>();
    }

    // Update is called once per frame
    void Update()
    {
        x+=5;
        y+=5;
        //Debug.Log("X: " + x + "Y: " + y);
        if (transform.position.magnitude > asteroidManager.GetDestroyRadius()) {
            Destroy(this.gameObject);
        }
        transform.localRotation = Quaternion.Euler(0, 0, y);

    }
}
