using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpaceController : MonoBehaviour
{
    LevelLoader levelLoader;
    bool canPress = true;

    // Start is called at the beginning
    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    public void Retry() 
    {
        if (canPress)
        {
            canPress = false;
            levelLoader.LoadLevelAtIndex(2);
        }
    }

    public void QuitGame()
    {
        if (canPress)
        {
            canPress = false;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }

    public void MainMenu() 
    {
        if (canPress) 
        {
            canPress = false;
            levelLoader.LoadLevelAtIndex(1);
        }
    }
}
