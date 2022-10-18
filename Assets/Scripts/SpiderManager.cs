using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderManager : MonoBehaviour
{
    private Animator[] _spiderAnimators;
    public GameObject[] spiders;
    public BackgroundMusicManager backgroundMusicManager;

    // Start is called before the first frame update
    void Start()
    {
        _spiderAnimators = new Animator[4];
        for (int i = 0; i < spiders.Length; i++)
        {
            _spiderAnimators[i] = spiders[i].GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ScaredState()
    {
        StartCoroutine(SpiderStatesCoroutine());
    }

    private IEnumerator SpiderStatesCoroutine()
    {
        for (int i = 0; i < spiders.Length; i++)
        {
            _spiderAnimators[i].SetTrigger("scared");
        }
        backgroundMusicManager.SpidersScared();
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
    }
}