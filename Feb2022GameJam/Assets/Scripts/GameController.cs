using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private double totalTimer = 0;
    private double spawnTimer = 0;
    double spawnFrequency = 2;
    AsteroidManager asteroidManager;

    void Start()
    {
        asteroidManager = FindObjectOfType<AsteroidManager>();
    }

    private void SpawnCheck()
    {
        if (spawnTimer > spawnFrequency){
            spawnTimer = 0;
            asteroidManager.Spawn();
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

    private void DifficultyControl() {
        if (totalTimer > 10) {
            spawnFrequency = 1;
        } else if (totalTimer > 20) {
            spawnFrequency = 0.3;
        }
    }

    void Update()
    {
        //Debug.Log("Hi");
        totalTimer = Time.time;
        spawnTimer += Time.deltaTime;
        SpawnCheck();
        DifficultyControl();
    }
}
