using System.Collections.Generic;
using UnityEngine;

namespace BeastLinkBattle.Grid.Service.Rules
{
    // StandardOnetMatchRule là quy tắc ghép đôi cơ bản trong trò chơi, cho phép người chơi ghép đôi hai ô nếu chúng có thể được kết nối bằng
    //  một đường thẳng hoặc một đường gấp khúc với tối đa 2 điểm gập khúc, mà không có chướng ngại vật nào nằm trên đường đi đó.
    public class StandardOnetMatchRule : IMatchRule
    {
        public List<Vector2Int> FindPath(Vector2Int p1, Vector2Int p2, IGridService gridService)
        {
            // 1. Kiểm tra đường thẳng (0 điểm gập khúc)
            if (p1.y == p2.y && CheckLineX(gridService, p1.y, p1.x, p2.x))
                return new List<Vector2Int> { p1, p2 };

            if (p1.x == p2.x && CheckLineY(gridService, p1.x, p1.y, p2.y))
                return new List<Vector2Int> { p1, p2 };

            // 2. Kiểm tra hình L (1 điểm gập khúc)
            Vector2Int corner1 = new Vector2Int(p1.x, p2.y);
            Vector2Int corner2 = new Vector2Int(p2.x, p1.y);

            if (gridService.IsWalkable(corner1) &&
                CheckLineY(gridService, p1.x, p1.y, p2.y) &&
                CheckLineX(gridService, p2.y, p1.x, p2.x))
                return new List<Vector2Int> { p1, corner1, p2 };

            if (gridService.IsWalkable(corner2) &&
                CheckLineX(gridService, p1.y, p1.x, p2.x) &&
                CheckLineY(gridService, p2.x, p1.y, p2.y))
                return new List<Vector2Int> { p1, corner2, p2 };

            // 3. Kiểm tra hình Z hoặc U (2 điểm gập khúc) 
            // bằng cách quét qua tất cả các cột và hàng để tìm 2 điểm trung gian có thể kết nối p1 và p2 với 2 đoạn thẳng.
            for (int x = 0; x < gridService.Width; x++)
            {
                // Bỏ qua nếu trùng với cột của p1 hoặc p2 để tránh kiểm tra lại đường thẳng đã kiểm tra ở bước 1
                if (x == p1.x || x == p2.x) continue;

                Vector2Int c1 = new Vector2Int(x, p1.y);
                Vector2Int c2 = new Vector2Int(x, p2.y);

                if (gridService.IsWalkable(c1) && gridService.IsWalkable(c2))
                {
                    if (CheckLineX(gridService, p1.y, p1.x, x) &&
                        CheckLineY(gridService, x, p1.y, p2.y) &&
                        CheckLineX(gridService, p2.y, x, p2.x))
                    {
                        return new List<Vector2Int> { p1, c1, c2, p2 };
                    }
                }
            }

            // Quét theo hàng tương tự để tìm các điểm trung gian trên trục Y
            for (int y = 0; y < gridService.Height; y++)
            {
                //  Bỏ qua nếu trùng với hàng của p1 hoặc p2 để tránh kiểm tra lại đường thẳng đã kiểm tra ở bước 1
                if (y == p1.y || y == p2.y) continue;

                Vector2Int c1 = new Vector2Int(p1.x, y);
                Vector2Int c2 = new Vector2Int(p2.x, y);

                if (gridService.IsWalkable(c1) && gridService.IsWalkable(c2))
                {
                    if (CheckLineY(gridService, p1.x, p1.y, y) &&
                        CheckLineX(gridService, y, p1.x, p2.x) &&
                        CheckLineY(gridService, p2.x, y, p2.y))
                    {
                        return new List<Vector2Int> { p1, c1, c2, p2 };
                    }
                }
            }

            // Nếu không tìm được đường nào hợp lệ, trả về null
            return null;
        }

        // --- HELPER ---
        private bool CheckLineX(IGridService gridService, int y, int x1, int x2)
        {
            int min = Mathf.Min(x1, x2);
            int max = Mathf.Max(x1, x2);

            for (int x = min + 1; x < max; x++)
            {
                if (!gridService.IsWalkable(new Vector2Int(x, y)))
                {
                    return false; 
                }
            }
            return true; 
        }

        private bool CheckLineY(IGridService gridService, int x, int y1, int y2)
        {
            int min = Mathf.Min(y1, y2);
            int max = Mathf.Max(y1, y2);

            for (int y = min + 1; y < max; y++)
            {
                if (!gridService.IsWalkable(new Vector2Int(x, y)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}