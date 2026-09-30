using UnityEngine;

namespace BeastLinkBattle.Grid.Service
{

    // GridTransform là một lớp tiện ích cung cấp các phương thức để chuyển đổi giữa tọa độ lưới (Grid)
    //  và tọa độ thế giới (World) trong trò chơi.
    public static class GridTransform
    {
        public static Vector3 GridToWorld(Vector2Int g, float cellSize)
        {
            return new Vector3(g.x * cellSize, g.y * cellSize, 0);
        }

        public static Vector2Int WorldToGrid(Vector3 w, float cellSize)
        {
            float halfCell = cellSize / 2f;
            return new Vector2Int(
                Mathf.FloorToInt((w.x + halfCell) / cellSize),
                Mathf.FloorToInt((w.y + halfCell) / cellSize)
            );
        }
    }
}