using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public SpiderManager spiderManager;
    private Animator _animator;
    public BackgroundMusicManager backgroundMusicManager;
    public ScoreController scoreController;

    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (spiderManager.areScared)
            {
                StartCoroutine(DeadCoroutine());
            }
        }
    }

    private IEnumerator DeadCoroutine()
    {
        _animator.SetTrigger("dead");
        backgroundMusicManager.SpiderDead();
        scoreController.score += 300;
        yield return new WaitForSeconds(5.0f);
        _animator.ResetTrigger("dead");
    }
}