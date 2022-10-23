using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpiderManager : MonoBehaviour
{
    public BackgroundMusicManager backgroundMusicManager;
    public GameObject timer;
    private Text _timerText;
    public float time;
    public GhostController[] spiders;

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
        if (AnyDead())
        {
            backgroundMusicManager.SpiderDead();
        }
        else if (AreScared())
        {
            backgroundMusicManager.SpidersScared();
        }
        else
        {
            backgroundMusicManager.SpidersNormal();
        }
    }

    public void ScaredState()
    {
        StartCoroutine(SpiderStatesCoroutine());
    }

    private IEnumerator SpiderStatesCoroutine()
    {
        state = State.Scared;
        time = Time.time;
        timer.SetActive(true);
        yield return new WaitForSeconds(7.0f);
        state = State.Recovering;
        yield return new WaitForSeconds(3.0f);
        state = State.Walking;
        timer.SetActive(false);
    }

    public bool AreScared()
    {
        return state is State.Scared or State.Recovering;
    }

    public bool AnyDead()
    {
        for (int i = 0; i < spiders.Length; i++)
        {
            if (spiders[i].isDead)
            {
                return true;
            }
        }

        return false;
    }
}