using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private double totalTimer = 0;
    private double spawnTimer = 0;
    [SerializeField] double spawnFrequency = 4;
    [SerializeField] double spawnTimeMinimum = 0.5; //in seconds
    [SerializeField] double spawnFrequencyScalar;
    [SerializeField] GameObject pauseMenu;
    AsteroidManager asteroidManager;
    LevelLoader levelLoader;

    PlayerController playerController;
    AudioManager audioManager;
    bool canSpawn;
    bool canMove;

    void Start()
    {
        asteroidManager = FindObjectOfType<AsteroidManager>();
        levelLoader = FindObjectOfType<LevelLoader>();
        playerController = FindObjectOfType<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
        GameObject.FindGameObjectWithTag("Menu").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("Retry").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("GameOver").GetComponent<SpriteRenderer>().enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        GameObject.FindGameObjectWithTag("MusicTrack").GetComponent<AudioSource>().Stop();
        
        //PausePlayGame();
        canSpawn = false;
        canMove = false;
        Invoke("RenderDelayedEnd", 4f);
        // playerController.setCanMove(false);
        /*float timeUntilEndMusic = Time.time + 4;
        while (Time.time < timeUntilEndMusic) {
 
        }*/
        //FindObjectOfType<AudioManager>().Play("loseTrack");
        //GameObject.FindGameObjectWithTag("MusicTrack").GetComponent<AudioSource>().Stop();
    }

    void RenderDelayedEnd() {
        FindObjectOfType<AudioManager>().Play("loseTrack");
        GameObject.FindGameObjectWithTag("GameOver").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.FindGameObjectWithTag("Menu").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("Retry").GetComponent<Image>().enabled = true;
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
            //Debug.Log("Asteroids are currently spawning every " + spawnFrequency + " seconds.");
            
        }
        if (spawnFrequency <= spawnTimeMinimum) {
            //Debug.Log("Reached the minimum at " + totalTimer + " seconds.");
        }
        //Debug.Log(totalTimer + " seconds have passed.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canSpawn) 
        {
            PausePlayGame();
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);

            if (Cursor.visible) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        totalTimer = Time.timeSinceLevelLoad;
        spawnTimer += Time.deltaTime;
        Debug.Log((int) Time.fixedTime);
        SpawnCheck();
        DifficultyControl();
    }

    public bool IsGamePlaying() {
        return canMove;
    }
}
