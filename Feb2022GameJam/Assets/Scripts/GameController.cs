using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private double totalTimer = 0;
    private double spawnTimer = 0;
    [SerializeField] double spawnFrequency = 0.01;
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

    public void PausePlayGame()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
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
