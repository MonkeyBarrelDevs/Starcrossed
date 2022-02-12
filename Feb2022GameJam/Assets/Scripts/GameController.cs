using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private float totalTimer = 0;
    private float spawnTimer = 0;
    private int spawnFrequency = 10;

    void Start()
    {
        
    }

    private void SpawnCheck()
    {
        if (spawnTimer > spawnFrequency)
        {
            spawnTimer -= spawnFrequency;
            //call spawn function in asteroidSpawner
        }
    }

    void Update()
    {
        totalTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        //if (checked earth controller to see if dead)
        //{
        //tell screen manager to end screen
        //}
        SpawnCheck();
    }
}
