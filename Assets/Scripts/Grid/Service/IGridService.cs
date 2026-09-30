using BeastLinkBattle.Grid.Data;
using System.Collections.Generic;
using UnityEngine;

namespace BeastLinkBattle.Grid.Service
{
    public interface IGridService
    {
        int Width { get; }
        int Height { get; }
        Tile GetTile(Vector2Int pos);
        bool IsWalkable(Vector2Int pos);
        bool IsValid(Vector2Int pos);
        List<Vector2Int> GetEmptyPositions();
        List<Vector2Int> GetOccupiedPositions();
        void ClearTileContent(Vector2Int pos);
        void SetTileContent(Vector2Int pos, ITileContent content);
        int GetTotalPlayableTiles();
    }
}