using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener _tweener;
    private Animator _animator;
    private AudioSource _audioSource;
    private static float Speed = 0.5f;
    public LevelManager levelManager;
    public AudioClip movingClip;
    public AudioClip eatingClip;

    private enum UserInput
    {
        Up,
        Left,
        Down,
        Right
    }
    
    // ReSharper disable once InconsistentNaming
    private UserInput lastInput = UserInput.Right;
    // ReSharper disable once InconsistentNaming
    private UserInput currentInput = UserInput.Right;

    // Start is called before the first frame update
    void Start()
    {
        _tweener = gameObject.GetComponent<Tweener>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        transform.position = new Vector3(LevelManager.TileSize, -LevelManager.TileSize, 0);
        
    }

    void Update()
    {
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
            if(IsWalkable(lastInput))
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
                _audioSource.Stop();
            }
        }
   
    }
    private bool IsWalkable(UserInput input)
    {
        return GetNeighbor(input) is LevelManager.Tile.Empty or LevelManager.Tile.StandardPellet or LevelManager.Tile.PowerPellet;
    }

    private LevelManager.Tile GetNeighbor(UserInput input)
    {
        int j = Mathf.RoundToInt(transform.position.x / LevelManager.TileSize);
        int i = Mathf.RoundToInt(-transform.position.y / LevelManager.TileSize);
        LevelManager.Tile tileNeighbour = LevelManager.Tile.Empty;

        // TODO: handle teleport
        switch (input)
        {
            case UserInput.Up:
                tileNeighbour = levelManager.grid[i-1, j];
                break;
            case UserInput.Left:
                tileNeighbour = levelManager.grid[i, j-1];
                break;
            case UserInput.Down:
                tileNeighbour = levelManager.grid[i+1, j];
                break;
            case UserInput.Right:
                tileNeighbour = levelManager.grid[i, j+1];
                break;
        }

        return tileNeighbour;
    }

    private void MoveRight()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position + new Vector3(LevelManager.TileSize, 0, 0), Speed);
        _animator.SetTrigger("right");
    }

    private void MoveLeft()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position - new Vector3(LevelManager.TileSize, 0, 0), Speed);
        _animator.SetTrigger("left");
    }

    private void MoveDown()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position - new Vector3(0, LevelManager.TileSize, 0), Speed);
        _animator.SetTrigger("down");
    }

    private void MoveUp()
    {
        StartMovement();
        _tweener.AddTween(transform, transform.position, transform.position + new Vector3(0, LevelManager.TileSize, 0), Speed);
        _animator.SetTrigger("up");
    }

    private void StartMovement()
    {
        _animator.speed = 1.0f;
        if (GetNeighbor(currentInput) is LevelManager.Tile.StandardPellet or LevelManager.Tile.PowerPellet)
        {
            _audioSource.clip = eatingClip;
        }
        else _audioSource.clip = movingClip;
        
        _audioSource.Play();
        _animator.ResetTrigger("right");
        _animator.ResetTrigger("left");
        _animator.ResetTrigger("up");
        _animator.ResetTrigger("down");
    }
}