using System.Collections.Generic;
using UnityEngine;

namespace BeastLinkBattle.Grid.Service.Rules
{
    // Interface IMatchRule định nghĩa một quy tắc để tìm đường đi giữa hai vị trí trên lưới, 
    // có thể được sử dụng để xác định xem hai ô có thể được ghép đôi với nhau hay
    public interface IMatchRule
    {
        List<Vector2Int> FindPath(Vector2Int p1, Vector2Int p2, IGridService gridService);
    }
}