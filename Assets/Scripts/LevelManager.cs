using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] rawTiles;
    public GameObject player;

    private float _tileSize;

    private Tweener _tweener;
    private Animator _playerAnimator;
    private int _nextTween;
    private Vector3 _posTopLeft;
    private Vector3 _posTopRight;
    private Vector3 _posBottomLeft;
    private Vector3 _posBottomRight;

    private enum Tile
    {
        Empty,
        OutsideCornerTopRight,
        OutsideCornerDownRight,
        OutsideCornerTopLeft,
        OutsideCornerDownLeft,
        OutsideWallVertical,
        OutsideWallHorizontal,
        InsideCornerTopRight,
        InsideCornerDownRight,
        InsideCornerTopLeft,
        InsideCornerDownLeft,
        InsideWallVertical,
        InsideWallHorizontal,
        StandardPellet,
        PowerPellet,
        JunctionTop,
        JunctionDown,
        JunctionLeft,
        JunctionRight
    }

    // Start is called before the first frame update
    void Start()
    {
        Tile[,] tiles =
        {
            {
                Tile.OutsideCornerDownRight, Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal,
                Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal,
                Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal,
                Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal,
                Tile.OutsideWallHorizontal, Tile.JunctionDown
            },
            {
                Tile.OutsideWallVertical, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet,
                Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet,
                Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet,
                Tile.InsideWallVertical
            },
            {
                Tile.OutsideWallVertical, Tile.StandardPellet, Tile.InsideCornerDownRight, Tile.InsideWallHorizontal,
                Tile.InsideWallHorizontal, Tile.InsideCornerDownLeft, Tile.StandardPellet, Tile.InsideCornerDownRight,
                Tile.InsideWallHorizontal, Tile.InsideWallHorizontal, Tile.InsideWallHorizontal,
                Tile.InsideCornerDownLeft,
                Tile.StandardPellet, Tile.InsideWallVertical
            },
            {
                Tile.OutsideWallVertical, Tile.PowerPellet, Tile.InsideWallVertical, Tile.Empty, Tile.Empty,
                Tile.InsideWallVertical, Tile.StandardPellet, Tile.InsideWallVertical, Tile.Empty, Tile.Empty,
                Tile.Empty, Tile.InsideWallVertical,
                Tile.StandardPellet, Tile.InsideWallVertical
            },
            {
                Tile.OutsideWallVertical, Tile.StandardPellet, Tile.InsideCornerTopRight, Tile.InsideWallHorizontal,
                Tile.InsideWallHorizontal, Tile.InsideCornerTopLeft, Tile.StandardPellet, Tile.InsideCornerTopRight,
                Tile.InsideWallHorizontal, Tile.InsideWallHorizontal, Tile.InsideWallHorizontal,
                Tile.InsideCornerTopLeft,
                Tile.StandardPellet, Tile.InsideCornerTopRight
            },
            {
                Tile.OutsideWallVertical, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet,
                Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet,
                Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet
            },
            {
                Tile.OutsideWallVertical, Tile.StandardPellet, Tile.InsideCornerDownRight, Tile.InsideWallHorizontal,
                Tile.InsideWallHorizontal, Tile.InsideCornerDownLeft, Tile.StandardPellet, Tile.InsideCornerDownRight,
                Tile.InsideCornerDownLeft,
                Tile.StandardPellet, Tile.InsideCornerDownRight, Tile.InsideWallHorizontal, Tile.InsideWallHorizontal,
                Tile.InsideWallHorizontal
            },
            {
                Tile.OutsideWallVertical, Tile.StandardPellet, Tile.InsideCornerTopRight, Tile.InsideWallHorizontal,
                Tile.InsideWallHorizontal, Tile.InsideCornerTopLeft, Tile.StandardPellet, Tile.InsideWallVertical,
                Tile.InsideWallVertical,
                Tile.StandardPellet, Tile.InsideCornerTopRight, Tile.InsideWallHorizontal, Tile.InsideWallHorizontal,
                Tile.InsideCornerDownLeft
            },
            {
                Tile.OutsideWallVertical, Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet,
                Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet, Tile.InsideWallVertical,
                Tile.InsideWallVertical, Tile.StandardPellet,
                Tile.StandardPellet, Tile.StandardPellet, Tile.StandardPellet, Tile.InsideWallVertical
            },
            {
                Tile.OutsideCornerTopRight, Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal,
                Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal, Tile.OutsideCornerDownLeft, Tile.StandardPellet,
                Tile.InsideWallVertical, Tile.InsideCornerTopRight, Tile.InsideWallHorizontal,
                Tile.InsideWallHorizontal,
                Tile.InsideCornerDownLeft, Tile.Empty, Tile.InsideWallVertical
            },
            {
                Tile.Empty, Tile.Empty, Tile.Empty, Tile.Empty, Tile.Empty, Tile.OutsideWallVertical,
                Tile.StandardPellet, Tile.InsideWallVertical, Tile.InsideCornerDownRight, Tile.InsideWallHorizontal,
                Tile.InsideWallHorizontal, Tile.InsideCornerTopLeft, Tile.Empty, Tile.InsideCornerTopRight
            },
            {
                Tile.Empty, Tile.Empty, Tile.Empty, Tile.Empty, Tile.Empty, Tile.OutsideWallVertical,
                Tile.StandardPellet, Tile.InsideWallVertical, Tile.InsideWallVertical, Tile.Empty, Tile.Empty,
                Tile.Empty, Tile.Empty, Tile.Empty
            },
            {
                Tile.Empty, Tile.Empty, Tile.Empty, Tile.Empty, Tile.Empty, Tile.OutsideWallVertical,
                Tile.StandardPellet, Tile.InsideWallVertical, Tile.InsideWallVertical, Tile.Empty,
                Tile.InsideCornerDownRight, Tile.InsideWallHorizontal, Tile.InsideWallHorizontal, Tile.Empty
            },
            {
                Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal,
                Tile.OutsideWallHorizontal, Tile.OutsideWallHorizontal, Tile.OutsideCornerTopLeft, Tile.StandardPellet,
                Tile.InsideCornerTopRight, Tile.InsideCornerTopLeft, Tile.Empty, Tile.InsideWallVertical, Tile.Empty,
                Tile.Empty, Tile.Empty
            },
            {
                Tile.Empty, Tile.Empty, Tile.Empty, Tile.Empty, Tile.Empty, Tile.Empty, Tile.StandardPellet, Tile.Empty,
                Tile.Empty, Tile.Empty, Tile.InsideWallVertical, Tile.Empty, Tile.Empty, Tile.Empty
            },
        };

        var n = tiles.GetLength(0) * 2 - 1;
        var m = tiles.GetLength(1) * 2;
        var copiedTiles = new Tile[n, m];
        for (var i = 0; i < tiles.GetLength(0); i++)
        {
            for (var j = 0; j < tiles.GetLength(1); j++)
            {
                copiedTiles[i, j] = tiles[i, j];
                copiedTiles[(n - 1) - i, j] = MirrorY(tiles[i, j]);
                copiedTiles[i, (m - 1) - j] = MirrorX(tiles[i, j]);
                copiedTiles[(n - 1) - i, (m - 1) - j] = MirrorX(MirrorY(tiles[i, j]));
            }
        }

        _tileSize = rawTiles[0].GetComponent<SpriteRenderer>().bounds.size.x;

        for (var i = 0; i < copiedTiles.GetLength(0); i++)
        {
            for (var j = 0; j < copiedTiles.GetLength(1); j++)
            {
                var tile = copiedTiles[i, j];
                var rawTileGameObject = GetGameObject(tile);

                if (rawTileGameObject == null) continue;
                var tileGameObject =
                    Instantiate(rawTileGameObject, new Vector3(j * _tileSize, -i * _tileSize, 0), Quaternion.identity);

                if (tile == Tile.OutsideCornerDownLeft)
                {
                    tileGameObject.transform.Rotate(0, 0, 180);
                }

                if (tile == Tile.OutsideCornerDownRight)
                {
                    tileGameObject.transform.Rotate(0, 0, 270);
                }

                if (tile == Tile.OutsideCornerTopLeft)
                {
                    tileGameObject.transform.Rotate(0, 0, 90);
                }

                if (tile == Tile.OutsideWallHorizontal)
                {
                    tileGameObject.transform.Rotate(0, 0, 270);
                }

                if (tile == Tile.InsideCornerDownLeft)
                {
                    tileGameObject.transform.Rotate(0, 0, 180);
                }

                if (tile == Tile.InsideCornerDownRight)
                {
                    tileGameObject.transform.Rotate(0, 0, 270);
                }

                if (tile == Tile.InsideCornerTopLeft)
                {
                    tileGameObject.transform.Rotate(0, 0, 90);
                }

                if (tile == Tile.InsideWallHorizontal)
                {
                    tileGameObject.transform.Rotate(0, 0, 270);
                }

                if (tile == Tile.JunctionTop)
                {
                    tileGameObject.transform.Rotate(0, 0, 180);
                }

                if (tile == Tile.JunctionLeft)
                {
                    tileGameObject.transform.Rotate(0, 0, 270);
                }

                if (tile == Tile.JunctionRight)
                {
                    tileGameObject.transform.Rotate(0, 0, 90);
                }
            }
        }

        foreach (var rawTile in rawTiles)
        {
            rawTile.SetActive(false);
        }

        _tweener = gameObject.GetComponent<Tweener>();
        _playerAnimator = player.GetComponent<Animator>();
        _posTopLeft = new Vector3(_tileSize, -_tileSize, 0);
        _posTopRight = new Vector3(_tileSize * 6, -_tileSize, 0);
        _posBottomLeft = new Vector3(_tileSize, -_tileSize * 5, 0);
        _posBottomRight = new Vector3(_tileSize * 6, -_tileSize * 5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_tweener.TweenExists(player.transform)) return;

        ResetTriggers();

        switch (_nextTween)
        {
            case 0:
                _tweener.AddTween(player.transform, _posTopLeft, _posTopRight, 2.5f);
                _playerAnimator.SetTrigger("right");
                break;
            case 1:
                _tweener.AddTween(player.transform, _posTopRight, _posBottomRight, 2.0f);
                _playerAnimator.SetTrigger("down");
                break;
            case 2:
                _tweener.AddTween(player.transform, _posBottomRight, _posBottomLeft, 2.5f);
                _playerAnimator.SetTrigger("left");
                break;
            case 3:
                _tweener.AddTween(player.transform, _posBottomLeft, _posTopLeft, 2.0f);
                _playerAnimator.SetTrigger("up");
                break;
        }

        _nextTween = (_nextTween + 1) % 4;
    }

    private void ResetTriggers()
    {
        _playerAnimator.ResetTrigger("right");
        _playerAnimator.ResetTrigger("left");
        _playerAnimator.ResetTrigger("up");
        _playerAnimator.ResetTrigger("down");
    }

    GameObject GetGameObject(Tile tile)
    {
        switch (tile)
        {
            case Tile.OutsideCornerDownLeft:
            case Tile.OutsideCornerDownRight:
            case Tile.OutsideCornerTopLeft:
            case Tile.OutsideCornerTopRight:
                return rawTiles[0];
            case Tile.OutsideWallHorizontal:
            case Tile.OutsideWallVertical:
                return rawTiles[1];
            case Tile.InsideCornerDownLeft:
            case Tile.InsideCornerDownRight:
            case Tile.InsideCornerTopLeft:
            case Tile.InsideCornerTopRight:
                return rawTiles[2];
            case Tile.InsideWallHorizontal:
            case Tile.InsideWallVertical:
                return rawTiles[3];
            case Tile.StandardPellet:
                return rawTiles[4];
            case Tile.PowerPellet:
                return rawTiles[5];
            case Tile.JunctionDown:
            case Tile.JunctionLeft:
            case Tile.JunctionRight:
            case Tile.JunctionTop:
                return rawTiles[6];
            default: return null;
        }
    }

    private Tile MirrorX(Tile tile)
    {
        if (tile == Tile.OutsideCornerDownLeft)
        {
            return Tile.OutsideCornerDownRight;
        }

        if (tile == Tile.OutsideCornerDownRight)
        {
            return Tile.OutsideCornerDownLeft;
        }

        if (tile == Tile.OutsideCornerTopLeft)
        {
            return Tile.OutsideCornerTopRight;
        }

        if (tile == Tile.OutsideCornerTopRight)
        {
            return Tile.OutsideCornerTopLeft;
        }

        if (tile == Tile.InsideCornerDownLeft)
        {
            return Tile.InsideCornerDownRight;
        }

        if (tile == Tile.InsideCornerDownRight)
        {
            return Tile.InsideCornerDownLeft;
        }

        if (tile == Tile.InsideCornerTopLeft)
        {
            return Tile.InsideCornerTopRight;
        }

        if (tile == Tile.InsideCornerTopRight)
        {
            return Tile.InsideCornerTopLeft;
        }

        if (tile == Tile.JunctionLeft)
        {
            return Tile.JunctionRight;
        }

        if (tile == Tile.JunctionRight)
        {
            return Tile.JunctionLeft;
        }

        return tile;
    }

    private Tile MirrorY(Tile tile)
    {
        if (tile == Tile.OutsideCornerDownLeft)
        {
            return Tile.OutsideCornerTopLeft;
        }

        if (tile == Tile.OutsideCornerDownRight)
        {
            return Tile.OutsideCornerTopRight;
        }

        if (tile == Tile.OutsideCornerTopLeft)
        {
            return Tile.OutsideCornerDownLeft;
        }

        if (tile == Tile.OutsideCornerTopRight)
        {
            return Tile.OutsideCornerDownRight;
        }

        if (tile == Tile.InsideCornerDownLeft)
        {
            return Tile.InsideCornerTopLeft;
        }

        if (tile == Tile.InsideCornerDownRight)
        {
            return Tile.InsideCornerTopRight;
        }

        if (tile == Tile.InsideCornerTopLeft)
        {
            return Tile.InsideCornerDownLeft;
        }

        if (tile == Tile.InsideCornerTopRight)
        {
            return Tile.InsideCornerDownRight;
        }

        if (tile == Tile.JunctionTop)
        {
            return Tile.JunctionDown;
        }

        if (tile == Tile.JunctionDown)
        {
            return Tile.JunctionTop;
        }

        return tile;
    }
}
