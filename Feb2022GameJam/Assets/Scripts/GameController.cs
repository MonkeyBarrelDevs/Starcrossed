using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private double totalTimer = 0;
    private double spawnTimer = 0;
    [SerializeField] double spawnFrequency = 2;
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
