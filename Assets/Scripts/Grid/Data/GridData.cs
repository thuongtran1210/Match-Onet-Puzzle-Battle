using UnityEngine;

namespace BeastLinkBattle.Grid.Data
{
    public class GridData
    {
        private Tile[,] grid;
        private GridShape shape;

        public int Width => shape.width + 2;
        public int Height => shape.height + 2;

        public GridData(GridShape shape)
        {
            this.shape = shape;
            grid = new Tile[Width, Height];

            Initialize();
        }

        private void Initialize()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    Tile tile;

                    if (IsBorder(x, y))
                    {
                        tile = new Tile(pos, TileState.Border);
                    }
                    else
                    {
                        int shapeX = x - 1;
                        int shapeY = shape.height - 1 - (y - 1);

                        var cell = shape.GetCell(shapeX, shapeY);

                        tile = new Tile(pos, ConvertCell(cell));
                    }

                    grid[x, y] = tile;
                }
            }
        }

        public bool IsInBounds(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < Width &&
                   pos.y >= 0 && pos.y < Height;
        }

        public Tile GetTile(Vector2Int pos)
        {
            if (!IsInBounds(pos))
                return null;

            return grid[pos.x, pos.y];
        }

        public void SetTileState(Vector2Int pos, TileState state)
        {
            if (!IsInBounds(pos)) return;

            grid[pos.x, pos.y].SetState(state);
        }

        public bool IsValidCell(Vector2Int pos)
        {
            if (!IsInBounds(pos)) return false;

            return grid[pos.x, pos.y].State != TileState.Invalid;
        }

        public bool IsWalkable(Vector2Int pos)
        {
            var state = grid[pos.x, pos.y].State;
            return state == TileState.Empty || state == TileState.Border;
        }

        public bool IsBlocking(Vector2Int pos)
        {
            var state = grid[pos.x, pos.y].State;
            return state == TileState.Blocked || state == TileState.Occupied;
        }
        private bool IsBorder(int x, int y)
        {
            return x == 0 || y == 0 || x == Width - 1 || y == Height - 1;
        }


        private TileState ConvertCell(GridShapeCell cell)
        {
            return cell switch
            {
                GridShapeCell.Invalid => TileState.Invalid,
                GridShapeCell.Blocked => TileState.Blocked,
                _ => TileState.Empty
            };
        }


    }
}