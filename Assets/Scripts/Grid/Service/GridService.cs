using BeastLinkBattle.Grid.Data;
using System.Collections.Generic;
using UnityEngine;
namespace BeastLinkBattle.Grid.Service
{
    // GridService là một lớp triển khai của IGridService, cung cấp các phương thức để tương tác với lưới ô trong trò chơi. 
    // Nó quản lý trạng thái của các ô, cho phép kiểm tra tính hợp lệ của vị trí,
    // lấy danh sách các vị trí trống và đã chiếm đóng, cũng như thiết lập nội dung cho các ô.
    public class GridService : IGridService
    {
        private GridData grid;
        private List<Vector2Int> _cachedEmptyPositions = new List<Vector2Int>(100);
        private List<Vector2Int> _cachedOccupiedPositions = new List<Vector2Int>(100);

        public GridService(GridShape shape)
        {
            grid = new GridData(shape);
        }

        public Tile GetTile(Vector2Int pos) => grid.GetTile(pos);

        public bool IsWalkable(Vector2Int pos) => grid.IsWalkable(pos);

        public bool IsValid(Vector2Int pos) => grid.IsValidCell(pos);

        public List<Vector2Int> GetEmptyPositions()
        {
            _cachedEmptyPositions.Clear();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pos = new Vector2Int(x, y);
                    if (IsValid(pos) && GetTile(pos).State == TileState.Empty)
                    {
                        _cachedEmptyPositions.Add(pos);
                    }
                }
            }
            return _cachedEmptyPositions;
        }

        public void ClearTileContent(Vector2Int pos)
        {
            if (IsValid(pos))
            {
                grid.GetTile(pos).ClearContent();
            }
        }

        public void SetTileContent(Vector2Int pos, ITileContent content)
        {
            if (IsValid(pos))
            {
                grid.GetTile(pos).SetContent(content);
            }
        }
        public int GetTotalPlayableTiles()
        {
            int count = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pos = new Vector2Int(x, y);
                    if (IsValid(pos) && GetTile(pos).State != TileState.Blocked && GetTile(pos).State != TileState.Invalid)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public List<Vector2Int> GetOccupiedPositions()
        {
            _cachedOccupiedPositions.Clear();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pos = new Vector2Int(x, y);
                    if (IsValid(pos) && GetTile(pos).State == TileState.Occupied)
                    {
                        _cachedOccupiedPositions.Add(pos);
                    }
                }
            }
            return _cachedOccupiedPositions;
        }
        public int Width => grid.Width;
        public int Height => grid.Height;
    }

}

