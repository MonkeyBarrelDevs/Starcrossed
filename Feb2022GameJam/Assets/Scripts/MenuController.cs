using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] float TransitionThreshold = 0.5f;
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
        if (Input.GetMouseButtonDown(0)) {
            levelLoader.LoadNextLevel();
        }
    }
}
