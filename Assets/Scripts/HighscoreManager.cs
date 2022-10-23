using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    public GameObject highscore;
    public GameObject bestTime;
    
    // Start is called before the first frame update
    void Start()
    {
        Text highscoreText = highscore.GetComponent<Text>();
        Text timeText = bestTime.GetComponent<Text>();
        highscoreText.text = PlayerPrefs.GetInt("Score").ToString();
        float savedTime = PlayerPrefs.GetFloat("Time");
        int minutes = Mathf.FloorToInt(savedTime / 60);
        int seconds = Mathf.FloorToInt(savedTime) % 60;
        int milliseconds = Mathf.FloorToInt((Mathf.Floor(savedTime * 1000) % 1000) / 10);
        timeText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
