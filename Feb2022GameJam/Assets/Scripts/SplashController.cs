using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashController : MonoBehaviour
{
    [SerializeField] float TransitionThreshold = 2.5f;
    float timer = 0;
    LevelLoader levelLoader;

    // Start is called at the beginning
    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TransitionThreshold) 
        {
            levelLoader.LoadNextLevel();
        }
    }
}
