using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject countdown;
    public bool isPaused = true;
    private Text _countdownText;
    public BackgroundMusicManager backgroundMusicManager;

    // Start is called before the first frame update
    void Start()
    {
        _countdownText = countdown.GetComponent<Text>();
        StartCoroutine(CountdownCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
