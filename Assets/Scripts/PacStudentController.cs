using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener _tweener;
    private Animator _animator;
    private static float Speed = 0.5f;

    private enum UserInput
    {
        Up,
        Left,
        Down,
        Right
    }

    private UserInput _lastInput = UserInput.Right;

    // Start is called before the first frame update
    void Start()
    {
        _tweener = gameObject.GetComponent<Tweener>();
        _animator = gameObject.GetComponent<Animator>();
        transform.position = new Vector3(Grid.TileSize, -Grid.TileSize, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _lastInput = UserInput.Up;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _lastInput = UserInput.Left;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _lastInput = UserInput.Down;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _lastInput = UserInput.Right;
        }

        if (!_tweener.TweenExists(transform))
        {
            switch (_lastInput)
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
    }

    private void MoveRight()
    {
        ResetTriggers();
        _tweener.AddTween(transform, transform.position, transform.position + new Vector3(Grid.TileSize, 0, 0), Speed);
        _animator.SetTrigger("right");
    }

    private void MoveLeft()
    {
        ResetTriggers();
        _tweener.AddTween(transform, transform.position, transform.position - new Vector3(Grid.TileSize, 0, 0), Speed);
        _animator.SetTrigger("left");
    }

    private void MoveDown()
    {
        ResetTriggers();
        _tweener.AddTween(transform, transform.position, transform.position - new Vector3(0, Grid.TileSize, 0), Speed);
        _animator.SetTrigger("down");
    }

    private void MoveUp()
    {
        ResetTriggers();
        _tweener.AddTween(transform, transform.position, transform.position + new Vector3(0, Grid.TileSize, 0), Speed);
        _animator.SetTrigger("up");
    }

    private void ResetTriggers()
    {
        _animator.ResetTrigger("right");
        _animator.ResetTrigger("left");
        _animator.ResetTrigger("up");
        _animator.ResetTrigger("down");
    }
}