using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public GameObject[] rawTiles;
    public GameObject[] spiders;
    public static float TileSize = 128 / 100f;
    public Tile[,] grid;

    private enum RawTile
    {
        Empty = 0,
        OutsideCorner = 1,
        OutsideWall = 2,
        InsideCorner = 3,
        InsideWall = 4,
        StandardPellet = 5,
        PowerPellet = 6,
        Junction = 7
    }

    public enum Tile
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
        JunctionRight,
        Outside
    }

    public enum Direction
    {
        Up,
        Left,
        Down,
        Right,
        None
    }


    // Start is called before the first frame update
    void Start()
    {
        int[,] levelMap =
        {
            { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7 },
            { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4 },
            { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4 },
            { 2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4 },
            { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3 },
            { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },
            { 2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4 },
            { 2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3 },
            { 2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4 },
            { 1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4 },
            { 0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3 },
            { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0 },
            { 2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0 },
        };

        var tiles = new Tile[levelMap.GetLength(0), levelMap.GetLength(1)];

        for (var i = 0; i < levelMap.GetLength(0); i++)
        {
            for (var j = 0; j < levelMap.GetLength(1); j++)
            {
                var rawTile = (RawTile)levelMap[i, j];
                var neighborLeft = j > 0 ? tiles[i, j - 1] : Tile.Empty;
                var neighborTop = i > 0 ? tiles[i - 1, j] : Tile.Empty;
                var neighborTopIsOutsideWall =
                    neighborTop is Tile.OutsideWallVertical or Tile.OutsideCornerDownRight
                        or Tile.OutsideCornerDownLeft or Tile.JunctionRight or Tile.JunctionLeft;
                var neighborLeftIsOutsideWall =
                    neighborLeft is Tile.OutsideWallHorizontal or Tile.OutsideCornerTopRight
                        or Tile.OutsideCornerDownRight or Tile.JunctionTop or Tile.JunctionDown;
                var neighborTopIsInsideWall =
                    neighborTop is Tile.InsideWallVertical or Tile.InsideCornerDownRight or Tile.InsideCornerDownLeft
                        or Tile.JunctionDown;
                var neighborLeftIsInsideWall =
                    neighborLeft is Tile.InsideWallHorizontal or Tile.InsideCornerTopRight
                        or Tile.InsideCornerDownRight or Tile.JunctionRight;

                switch (rawTile)
                {
                    case RawTile.Empty:
                        tiles[i, j] = Tile.Empty;
                        break;
                    case RawTile.OutsideCorner:
                    {
                        if (neighborTopIsOutsideWall && neighborLeftIsOutsideWall)
                        {
                            tiles[i, j] = Tile.OutsideCornerTopLeft;
                        }

                        if (neighborTopIsOutsideWall && !neighborLeftIsOutsideWall)
                        {
                            tiles[i, j] = Tile.OutsideCornerTopRight;
                        }

                        if (!neighborTopIsOutsideWall && neighborLeftIsOutsideWall)
                        {
                            tiles[i, j] = Tile.OutsideCornerDownLeft;
                        }

                        if (!neighborTopIsOutsideWall && !neighborLeftIsOutsideWall)
                        {
                            tiles[i, j] = Tile.OutsideCornerDownRight;
                        }

                        break;
                    }
                    case RawTile.OutsideWall:
                    {
                        if (!neighborTopIsOutsideWall && (neighborLeftIsOutsideWall || j == 0))
                        {
                            tiles[i, j] = Tile.OutsideWallHorizontal;
                        }

                        if ((neighborTopIsOutsideWall || i == 0) && !neighborLeftIsOutsideWall)
                        {
                            tiles[i, j] = Tile.OutsideWallVertical;
                        }

                        break;
                    }
                    case RawTile.InsideCorner:
                    {
                        if (neighborTopIsInsideWall && neighborLeftIsInsideWall)
                        {
                            tiles[i, j] = Tile.InsideCornerTopLeft;
                        }

                        if (neighborTopIsInsideWall && !neighborLeftIsInsideWall)
                        {
                            tiles[i, j] = Tile.InsideCornerTopRight;
                        }

                        if (!neighborTopIsInsideWall && neighborLeftIsInsideWall)
                        {
                            tiles[i, j] = Tile.InsideCornerDownLeft;
                        }

                        if (!neighborTopIsInsideWall && !neighborLeftIsInsideWall)
                        {
                            tiles[i, j] = Tile.InsideCornerDownRight;
                        }

                        break;
                    }
                    case RawTile.InsideWall:
                    {
                        if (!neighborTopIsInsideWall && (neighborLeftIsInsideWall || j == 0))
                        {
                            tiles[i, j] = Tile.InsideWallHorizontal;
                        }

                        if ((neighborTopIsInsideWall || i == 0) && !neighborLeftIsInsideWall)
                        {
                            tiles[i, j] = Tile.InsideWallVertical;
                        }

                        break;
                    }
                    case RawTile.StandardPellet:
                        tiles[i, j] = Tile.StandardPellet;
                        break;
                    case RawTile.PowerPellet:
                        tiles[i, j] = Tile.PowerPellet;
                        break;
                    case RawTile.Junction:
                    {
                        if (!neighborTopIsInsideWall && !neighborTopIsOutsideWall && neighborLeftIsOutsideWall)
                        {
                            tiles[i, j] = Tile.JunctionDown;
                        }

                        if (neighborTopIsInsideWall && neighborLeftIsOutsideWall)
                        {
                            tiles[i, j] = Tile.JunctionTop;
                        }

                        if (neighborTopIsOutsideWall && neighborLeftIsInsideWall)
                        {
                            tiles[i, j] = Tile.JunctionLeft;
                        }

                        if (neighborTopIsOutsideWall && !neighborLeftIsInsideWall && !neighborLeftIsOutsideWall)
                        {
                            tiles[i, j] = Tile.JunctionRight;
                        }

                        break;
                    }
                }
            }
        }

        var n = tiles.GetLength(0) * 2 - 1;
        var m = tiles.GetLength(1) * 2;
        grid = new Tile[n, m];
        for (var i = 0; i < tiles.GetLength(0); i++)
        {
            for (var j = 0; j < tiles.GetLength(1); j++)
            {
                grid[i, j] = tiles[i, j];
                grid[(n - 1) - i, j] = MirrorY(tiles[i, j]);
                grid[i, (m - 1) - j] = MirrorX(tiles[i, j]);
                grid[(n - 1) - i, (m - 1) - j] = MirrorX(MirrorY(tiles[i, j]));
            }
        }


        for (var i = 0; i < grid.GetLength(0); i++)
        {
            for (var j = 0; j < grid.GetLength(1); j++)
            {
                var tile = grid[i, j];
                var rawTileGameObject = GetGameObject(tile);

                if (rawTileGameObject == null) continue;
                var tileGameObject =
                    Instantiate(rawTileGameObject, new Vector3(j * TileSize, -i * TileSize, 0), Quaternion.identity);

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

        spiders[0].transform.position = new Vector3(TileSize * 12, -TileSize * 14, 0);
        spiders[1].transform.position = new Vector3(TileSize * 15, -TileSize * 14, 0);
        spiders[2].transform.position = new Vector3(TileSize * 12, -TileSize * 15, 0);
        spiders[3].transform.position = new Vector3(TileSize * 15, -TileSize * 15, 0);
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

    public (int i, int j) getIndices(Vector3 position)
    {
        int j = Mathf.RoundToInt(position.x / TileSize);
        int i = Mathf.RoundToInt(-position.y / TileSize);
        return (i, j);
    }

    public Tile GetNeighbor(Direction direction, Vector3 position)
    {
        var indices = getIndices(position);
        int i = indices.i;
        int j = indices.j;

        switch (direction)
        {
            case Direction.Up:

                i--;
                break;
            case Direction.Left:

                j--;
                break;
            case Direction.Down:

                i++;
                break;
            case Direction.Right:

                j++;
                break;
        }

        if (grid.GetLength(0) <= i || grid.GetLength(1) <= j || i < 0 || j < 0)
        {
            return Tile.Outside;
        }
        else
        {
            return grid[i, j];
        }
    }

    public bool IsWalkable(Direction direction, Vector3 position)
    {
        return GetNeighbor(direction, position) is Tile.Empty or Tile.StandardPellet
            or Tile.PowerPellet;
    }
}