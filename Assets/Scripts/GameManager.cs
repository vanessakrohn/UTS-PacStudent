using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject countdown;
    public bool isPaused = true;
    private Text _countdownText;
    public BackgroundMusicManager backgroundMusicManager;
    public GameObject timer;
    private Text _timerText;
    public float startTime;
    public float totalTime;
    private bool timerReady = false;
    public ScoreController score;

    // Start is called before the first frame update
    void Start()
    {
        _countdownText = countdown.GetComponent<Text>();
        _timerText = timer.GetComponent<Text>();
        StartCoroutine(CountdownCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (timerReady)
        {
           totalTime = Time.time - startTime;
            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime) % 60;
            int milliseconds = Mathf.FloorToInt((Mathf.Floor(totalTime * 1000) % 1000) / 10);
            _timerText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D2");
        }
    }

    public IEnumerator CountdownCoroutine()
    {
        _countdownText.text = "3";
        yield return new WaitForSeconds(1.0f);
        _countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);
        _countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);
        _countdownText.text = "GO!";
        yield return new WaitForSeconds(1.0f);
        countdown.SetActive(false);
        isPaused = false;
        backgroundMusicManager.SpidersNormal();
        startTime = Time.time;
        timerReady = true;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    public IEnumerator GameOverCoroutine()
    {
        _countdownText.text = "Game Over";
        countdown.SetActive(true);
        timerReady = false;
        isPaused = true;
        float previousBestTime = PlayerPrefs.GetFloat("Time");
        int previousHighscore = PlayerPrefs.GetInt("Score");
        if (score.score > previousHighscore || (score.score == previousHighscore && totalTime < previousBestTime))
        {
            PlayerPrefs.SetInt("Score", score.score);
            PlayerPrefs.SetFloat("Time", totalTime);
            PlayerPrefs.Save();
        }
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}