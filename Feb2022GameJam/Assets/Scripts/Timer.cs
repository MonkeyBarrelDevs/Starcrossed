using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    private int timeCurrent = 0;
    GameController gameController;
    [SerializeField] TMP_Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void DisplayTime() {
        float minutes = Mathf.FloorToInt(timeCurrent / 60); 
        float seconds = Mathf.FloorToInt(timeCurrent % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
  }

    // Update is called once per frame
    void Update() {

        if ((int) Time.fixedTime >= timeCurrent  && gameController.IsGamePlaying()) {
            timeCurrent = (int) Time.timeSinceLevelLoad;
            DisplayTime();

        }
    }
}
