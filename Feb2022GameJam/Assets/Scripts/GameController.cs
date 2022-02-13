using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private double totalTimer = 0;
    private double spawnTimer = 0;
    [SerializeField] double spawnFrequency = 4;
    [SerializeField] double spawnTimeMinimum = 0.5; //in seconds
    [SerializeField] double spawnFrequencyScalar;
    AsteroidManager asteroidManager;
    LevelLoader levelLoader;

    PlayerController playerController;
    bool canSpawn;
    bool canMove;

    void Start()
    {
        asteroidManager = FindObjectOfType<AsteroidManager>();
        levelLoader = FindObjectOfType<LevelLoader>();
        playerController = FindObjectOfType<PlayerController>();
        canSpawn = true;
        canMove = true;
        spawnFrequencyScalar = 0.006; // This will progress a scale of spawning every 4 seconds to 0.5 seconds in 8-11 minutes.
    }

    private void SpawnCheck()
    {
        if (spawnTimer > spawnFrequency && canSpawn){
            spawnTimer = 0;
            asteroidManager.Spawn();
        }
    }

    public bool MoveCheck() {
        return canMove;
    }

    public void FailGame() {
        //PausePlayGame();
        canSpawn = false;
        canMove = false;
       // playerController.setCanMove(false);
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
        if (spawnFrequency > spawnTimeMinimum) {
            spawnFrequency = (spawnFrequency - (Time.deltaTime * spawnFrequencyScalar));
            Debug.Log("Asteroids are currently spawning every " + spawnFrequency + " seconds.");
            
        }
        if (spawnFrequency <= spawnTimeMinimum) {
            Debug.Log("Reached the minimum at " + totalTimer + " seconds.");
        }
        Debug.Log(totalTimer + " seconds have passed.");
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
