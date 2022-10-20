using System.Collections;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Vector3 _spawnPosition;
    private Tweener _tweener;
    private Animator _animator;
    private AudioSource _audioSource;
    private static float Speed = 0.4f;
    public LevelManager levelManager;
    public AudioClip movingClip;
    public AudioClip eatingClip;
    public AudioClip bumpingClip;
    public AudioClip deadClip;
    public ParticleSystem dust;
    public ParticleSystem wallBump;
    public ParticleSystem deathMark;
    private bool _mayBump = false;
    public GameObject[] lifeIndicators;
    private int _remainingLifes = 3;
    public GameManager gameManager;
    public SpiderManager spiderManager;

    private enum UserInput
    {
        Up,
        Left,
        Down,
        Right,
        None
    }

    // ReSharper disable once InconsistentNaming
    private UserInput lastInput = UserInput.None;

    // ReSharper disable once InconsistentNaming
    private UserInput currentInput = UserInput.None;

    // Start is called before the first frame update
    void Start()
    {
        _tweener = gameObject.GetComponent<Tweener>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        _spawnPosition = new Vector3(LevelManager.TileSize, -LevelManager.TileSize, 0);
        transform.position = _spawnPosition;
        dust.Stop();
        deathMark.Stop();
    }

    void Update()
    {
        if (gameManager.isPaused)
        {
            lastInput = UserInput.None;
            currentInput = UserInput.None;
            dust.Stop();
            return;
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = UserInput.Up;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = UserInput.Left;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = UserInput.Down;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = UserInput.Right;
        }

        if (!_tweener.TweenExists(transform))
        {
            if (ShouldTeleport())
            {
                Teleport();
            }
            else
            {
                if (IsWalkable(lastInput))
                {
                    currentInput = lastInput;
                }

                if (IsWalkable(currentInput))
                {
                    switch (currentInput)
                    {
                        case UserInput.Up:
                            MoveUp();
                            break;
                        case UserInput.Left:
                            MoveLeft();
                            break;
                        case UserInput.Down:
                            MoveDown();
                            break;
                        case UserInput.Right:
                            MoveRight();
                            break;
                    }
                }
                else
                {
                    _animator.speed = 0.0f;
                    dust.Stop();
                    StartCoroutine(Bump());
                }
            }
        }
    }

    private bool IsWalkable(UserInput input)
    {
        return GetNeighbor(input) is LevelManager.Tile.Empty or LevelManager.Tile.StandardPellet
            or LevelManager.Tile.PowerPellet;
    }

    private bool ShouldTeleport()
    {
        int j = Mathf.RoundToInt(transform.position.x / LevelManager.TileSize);
        int i = Mathf.RoundToInt(-transform.position.y / LevelManager.TileSize);

        switch (lastInput)
        {
            case UserInput.Up:
                i--;
                break;
            case UserInput.Left:
                j--;
                break;
            case UserInput.Down:
                i++;
                break;
            case UserInput.Right:
                j++;
                break;
        }

        return levelManager.grid.GetLength(0) <= i || levelManager.grid.GetLength(1) <= j || i < 0 || j < 0;
    }

    private void Teleport()
    {
        transform.position =
            new Vector3(LevelManager.TileSize * (levelManager.grid.GetLength(1) - 1) - transform.position.x,
                transform.position.y);
    }

    private LevelManager.Tile GetNeighbor(UserInput input)
    {
        var indices = levelManager.getIndices(transform.position);
        int i = indices.i;
        int j = indices.j;
        LevelManager.Tile tileNeighbour = LevelManager.Tile.Empty;

        switch (input)
        {
            case UserInput.Up:
                tileNeighbour = levelManager.grid[i - 1, j];
                break;
            case UserInput.Left:
                tileNeighbour = levelManager.grid[i, j - 1];
                break;
            case UserInput.Down:
                tileNeighbour = levelManager.grid[i + 1, j];
                break;
            case UserInput.Right:
                tileNeighbour = levelManager.grid[i, j + 1];
                break;
        }

        return tileNeighbour;
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
        _animator.speed = 1.0f;
        if (GetNeighbor(currentInput) is LevelManager.Tile.StandardPellet or LevelManager.Tile.PowerPellet)
        {
            _audioSource.clip = eatingClip;
        }
        else
        {
            _audioSource.clip = movingClip;
        }

        _audioSource.Play();
        dust.Play();
        _mayBump = true;
        _animator.ResetTrigger("right");
        _animator.ResetTrigger("left");
        _animator.ResetTrigger("up");
        _animator.ResetTrigger("down");
    }

    private IEnumerator Bump()
    {
        if (_mayBump)
        {
            _mayBump = false;
            var bump = wallBump.transform.localPosition;
            if (currentInput is UserInput.Right or UserInput.Up)
            {
                bump.x = Mathf.Abs(bump.x);
            }
            else
            {
                bump.x = -Mathf.Abs(bump.x);
            }

            wallBump.transform.localPosition = bump;
            wallBump.Play();
            _audioSource.clip = bumpingClip;
            _audioSource.Play();
            yield return new WaitForSeconds(0.3f);
            wallBump.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spider"))
        {
            if (!spiderManager.areScared)
            {
                StartCoroutine(DeadCoroutine());
            }
        }
    }

    private IEnumerator DeadCoroutine()
    {
        if (!gameManager.isPaused)
        {
            _animator.SetBool("dead", true);
            gameManager.isPaused = true;
            _audioSource.clip = deadClip;
            _audioSource.Play();
            deathMark.Play();
            _remainingLifes--;
            if (_remainingLifes >= 0)
            {
                Destroy(lifeIndicators[_remainingLifes]);
                if (_remainingLifes == 0)
                {
                    gameManager.GameOver();
                }
            }
            yield return new WaitForSeconds(2.5f);
            
            if (_remainingLifes > 0)
            {
                transform.position = _spawnPosition;
            }
            deathMark.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _animator.SetBool("dead", false);
            gameManager.isPaused = false;
            _animator.SetTrigger("right");
        }
    }
}