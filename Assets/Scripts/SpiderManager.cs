using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderManager : MonoBehaviour
{
    private Animator[] _spiderAnimators;
    public GameObject[] spiders;
    public BackgroundMusicManager backgroundMusicManager;
    public GameObject timer;
    private Text timerText;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        _spiderAnimators = new Animator[4];
        for (int i = 0; i < spiders.Length; i++)
        {
            _spiderAnimators[i] = spiders[i].GetComponent<Animator>();
            timerText = timer.GetComponent<Text>();
        }
        timer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float remainingTime = Mathf.Ceil(10.0f - (Time.time - time));
        timerText.text = "Scared Spiders for: " + remainingTime.ToString();
    }

    public void ScaredState()
    {
        StartCoroutine(SpiderStatesCoroutine());
    }

    private IEnumerator SpiderStatesCoroutine()
    {
        time = Time.time;
        
        for (int i = 0; i < spiders.Length; i++)
        {
            _spiderAnimators[i].SetTrigger("scared");
        }
        backgroundMusicManager.SpidersScared();
        timer.SetActive(true);
        yield return new WaitForSeconds(7.0f);
        for (int i = 0; i < spiders.Length; i++)
        {
            _spiderAnimators[i].SetTrigger("recovering");
        }
        yield return new WaitForSeconds(3.0f);
        for (int i = 0; i < spiders.Length; i++)
        {
            _spiderAnimators[i].SetTrigger("left");
        }
        backgroundMusicManager.SpidersNormal();
        timer.SetActive(false);
    }
}