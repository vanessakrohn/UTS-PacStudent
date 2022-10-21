using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostController : MonoBehaviour
{
    public SpiderManager spiderManager;
    private Animator _animator;
    public BackgroundMusicManager backgroundMusicManager;
    public ScoreController scoreController;
    public LevelManager levelManager;
    private Tweener _tweener;
    private static float Speed = 0.4f;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _tweener = gameObject.GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isPaused)
        {
            return;
        }

        if (!_tweener.TweenExists(transform))
        {
            Spider3();
        }
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

    private void Spider3()
    {
        LevelManager.Direction[] directions =
        {
            LevelManager.Direction.Up, LevelManager.Direction.Down, LevelManager.Direction.Left,
            LevelManager.Direction.Right
        };
        ArrayList walkableDirections = new ArrayList();
        
        
        for (int i = 0; i < directions.Length; i++)
        {
            if(levelManager.IsWalkable(directions[i], transform.position))
            {
                Debug.Log(directions[i]);
                walkableDirections.Add(directions[i]);
            }
        }
        int randValue = Random.Range(0, walkableDirections.Count);
        LevelManager.Direction nextDirection = (LevelManager.Direction) walkableDirections[randValue];
        switch (nextDirection)
        {
            case LevelManager.Direction.Up:
                MoveUp();
                break;
            case LevelManager.Direction.Left:
                MoveLeft();
                break;
            case LevelManager.Direction.Down:
                MoveDown();
                break;
            case LevelManager.Direction.Right:
                MoveRight();
                break;
        }
    }
    
    private void MoveRight()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position + new Vector3(LevelManager.TileSize, 0, 0),
            Speed);
        _animator.SetTrigger("right");
    }

    private void MoveLeft()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position - new Vector3(LevelManager.TileSize, 0, 0),
            Speed);
        _animator.SetTrigger("left");
    }

    private void MoveDown()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position - new Vector3(0, LevelManager.TileSize, 0),
            Speed);
        _animator.SetTrigger("down");
    }

    private void MoveUp()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position + new Vector3(0, LevelManager.TileSize, 0),
            Speed);
        _animator.SetTrigger("up");
    }
    
    private void StartMovement()
    {
        
        _animator.ResetTrigger("right");
        _animator.ResetTrigger("left");
        _animator.ResetTrigger("up");
        _animator.ResetTrigger("down");
    }
}