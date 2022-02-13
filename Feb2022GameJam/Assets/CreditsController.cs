using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    LevelLoader levelLoader;
    bool canPress = true;

    // Start is called at the beginning
    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    public void ReturnToMenu()
    {
        if (canPress)
        {
            canPress = false;
            levelLoader.LoadLevelAtIndex(1);
        }
    }
}
