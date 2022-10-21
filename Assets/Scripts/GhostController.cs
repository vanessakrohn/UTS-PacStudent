using System;
using System.Collections;
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
    private LevelManager.Direction _blockedMoveDirection = LevelManager.Direction.None;
    public GameObject pacStudent;
    private Ghost4MovementGrid _ghost4MovementGrid;
    private bool _ghost4Flip = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _tweener = gameObject.GetComponent<Tweener>();
        _ghost4MovementGrid = new Ghost4MovementGrid();
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
            if (levelManager.InSpawnArea(transform.position))
            {
                Spider4();
                return;
            }

            if (spiderManager.areScared)
            {
                Spider1();
                return;
            }

            switch (gameObject.name)
            {
                case "Spider1":
                    Spider1();
                    break;
                case "Spider2":
                    Spider2();
                    break;
                case "Spider3":
                    Spider3();
                    break;
                case "Spider4":
                    Spider4();
                    break;
            }
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
        _animator.SetBool("dead", true);
        backgroundMusicManager.SpiderDead();
        scoreController.score += 300;
        yield return new WaitForSeconds(5.0f);
        _animator.SetBool("dead", false);
    }

    private void Spider1()
    {
        Vector3 spiderPos = transform.position;
        Vector3 pacStudentPos = pacStudent.transform.position;
        ArrayList validDirections = new ArrayList();

        if (spiderPos.x >= pacStudentPos.x)
        {
            validDirections.Add(LevelManager.Direction.Right);
        }

        if (spiderPos.x <= pacStudentPos.x)
        {
            validDirections.Add(LevelManager.Direction.Left);
        }

        if (spiderPos.y >= pacStudentPos.y)
        {
            validDirections.Add(LevelManager.Direction.Up);
        }

        if (spiderPos.y <= pacStudentPos.y)
        {
            validDirections.Add(LevelManager.Direction.Down);
        }

        if (MoveRandomly(validDirections) == LevelManager.Direction.None)
        {
            Spider3();
        }
    }

    private void Spider2()
    {
        Vector3 spiderPos = transform.position;
        Vector3 pacStudentPos = pacStudent.transform.position;
        ArrayList validDirections = new ArrayList();

        if (spiderPos.x <= pacStudentPos.x)
        {
            validDirections.Add(LevelManager.Direction.Right);
        }

        if (spiderPos.x >= pacStudentPos.x)
        {
            validDirections.Add(LevelManager.Direction.Left);
        }

        if (spiderPos.y <= pacStudentPos.y)
        {
            validDirections.Add(LevelManager.Direction.Up);
        }

        if (spiderPos.y >= pacStudentPos.y)
        {
            validDirections.Add(LevelManager.Direction.Down);
        }

        if (MoveRandomly(validDirections) == LevelManager.Direction.None)
        {
            Spider3();
        }
    }

    private void Spider3()
    {
        ArrayList directions = new ArrayList()
        {
            LevelManager.Direction.Up, LevelManager.Direction.Down, LevelManager.Direction.Left,
            LevelManager.Direction.Right
        };
        if (MoveRandomly(directions) == LevelManager.Direction.None)
        {
            Move(_blockedMoveDirection);
        }
    }

    private void Spider4()
    {
        var indices = levelManager.GetIndices(transform.position);
        var nextDirection = _ghost4MovementGrid.movementMap[indices.i, indices.j];

        if (_ghost4Flip)
        {
            if (indices.i == 14)
            {
                if (indices.j == 6)
                {
                    Move(LevelManager.Direction.Up);
                    _ghost4Flip = false;
                    return;
                }

                if (indices.j == 21)
                {
                    Move(LevelManager.Direction.Down);
                    _ghost4Flip = false;
                    return;
                }
            }

            switch (nextDirection)
            {
                case LevelManager.Direction.Left:
                    Move(LevelManager.Direction.Right);
                    return;
                case LevelManager.Direction.Right:
                    Move(LevelManager.Direction.Left);
                    return;
            }
        }

        if (nextDirection == _blockedMoveDirection)
        {
            _ghost4Flip = true;
        }

        Move(nextDirection);
    }

    private LevelManager.Direction MoveRandomly(ArrayList directions)
    {
        ArrayList walkableDirections = new ArrayList();
        for (int i = 0; i < directions.Count; i++)
        {
            LevelManager.Direction direction = (LevelManager.Direction)directions[i];
            if (!levelManager.IsWalkable(direction, transform.position)) continue;
            if (direction == _blockedMoveDirection) continue;
            if (!levelManager.InSpawnArea(transform.position) &&
                levelManager.IsNeighborInSpawnArea(direction, transform.position)) continue;

            walkableDirections.Add(directions[i]);
        }

        if (walkableDirections.Count == 0)
        {
            return LevelManager.Direction.None;
        }

        int randValue = Random.Range(0, walkableDirections.Count);
        LevelManager.Direction nextDirection = (LevelManager.Direction)walkableDirections[randValue];
        Move(nextDirection);
        return nextDirection;
    }

    private void Move(LevelManager.Direction nextDirection)
    {
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
        _blockedMoveDirection = LevelManager.Direction.Left;
    }

    private void MoveLeft()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position - new Vector3(LevelManager.TileSize, 0, 0),
            Speed);
        _animator.SetTrigger("left");
        _blockedMoveDirection = LevelManager.Direction.Right;
    }

    private void MoveDown()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position - new Vector3(0, LevelManager.TileSize, 0),
            Speed);
        _animator.SetTrigger("down");
        _blockedMoveDirection = LevelManager.Direction.Up;
    }

    private void MoveUp()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position + new Vector3(0, LevelManager.TileSize, 0),
            Speed);
        _animator.SetTrigger("up");
        _blockedMoveDirection = LevelManager.Direction.Down;
    }

    private void StartMovement()
    {
        _animator.ResetTrigger("right");
        _animator.ResetTrigger("left");
        _animator.ResetTrigger("up");
        _animator.ResetTrigger("down");
    }
}
