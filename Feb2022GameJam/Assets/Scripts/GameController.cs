using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private double totalTimer = 0;
    private double spawnTimer = 0;
    [SerializeField] double spawnFrequency = 0.1;
    AsteroidManager asteroidManager;

    void Start()
    {
        asteroidManager = FindObjectOfType<AsteroidManager>();
    }

    private void SpawnCheck()
    {
        if (spawnTimer > spawnFrequency)
        {
            spawnTimer -= spawnFrequency;
            asteroidManager.Spawn();
            Debug.Log("Hello");
        }
    }

    void Update()
    {
        Debug.Log("Hi");
        totalTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        //if (checked earth controller to see if dead)
        //{
        //tell screen manager to end screen
        //}
        SpawnCheck();
    }
}
