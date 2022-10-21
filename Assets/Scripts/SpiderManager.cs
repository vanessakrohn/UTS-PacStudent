using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderManager : MonoBehaviour
{
    public BackgroundMusicManager backgroundMusicManager;
    public GameObject timer;
    private Text _timerText;
    public float time;

    public enum State
    {
        Walking,
        Scared,
        Recovering
    }

    public State state = State.Walking;

    // Start is called before the first frame update
    void Start()
    {
        _timerText = timer.GetComponent<Text>();
        timer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float remainingTime = Mathf.Ceil(10.0f - (Time.time - time));
        _timerText.text = "Scared Spiders for: " + remainingTime.ToString();
    }

    public void ScaredState()
    {
        StartCoroutine(SpiderStatesCoroutine());
    }

    private IEnumerator SpiderStatesCoroutine()
    {
        state = State.Scared;
        time = Time.time;
        backgroundMusicManager.SpidersScared();
        timer.SetActive(true);
        yield return new WaitForSeconds(7.0f);
        state = State.Recovering;
        yield return new WaitForSeconds(3.0f);
        state = State.Walking;
        backgroundMusicManager.SpidersNormal();
        timer.SetActive(false);
    }

    public bool AreScared()
    {
        return state is State.Scared or State.Recovering;
    }
}
