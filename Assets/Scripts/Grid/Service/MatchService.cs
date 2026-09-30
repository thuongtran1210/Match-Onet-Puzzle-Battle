using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Grid.Service.Rules;
using System.Collections.Generic;
using UnityEngine;

namespace BeastLinkBattle.Grid.Service
{
    // MatchService là một lớp triển khai của IMatchService, chịu trách nhiệm kiểm tra xem hai ô có thể được ghép đôi với nhau hay không dựa trên các quy tắc đã định nghĩa.
    // Nó sử dụng IGridService để truy cập thông tin về lưới và các ô, và một danh sách các IMatchRule để xác định các cách ghép đôi hợp lệ giữa hai ô.
    public class MatchService : IMatchService
    {
        private readonly IGridService _gridService;
        private readonly List<IMatchRule> _rules; 
        public MatchService(IGridService gridService, List<IMatchRule> rules)
        {
            _gridService = gridService;
            _rules = rules;
        }

        public List<Vector2Int> CheckMatch(Vector2Int p1, Vector2Int p2)
        {
            if (!CheckIsMatch(p1, p2))
            {
                return null;
            }

            foreach (var rule in _rules)
            {
                var path = rule.FindPath(p1, p2, _gridService);
                if (path != null && path.Count > 0)
                {
                    return path;
                }
            }
            return null;
        }

        private bool CheckIsMatch(Vector2Int p1, Vector2Int p2)
        {
            if (p1 == p2) return false;
            Tile t1 = _gridService.GetTile(p1);
            Tile t2 = _gridService.GetTile(p2);
            if (t1 == null || t2 == null || t1.Content == null || t2.Content == null) return false;
            return t1.Content.IsMatchWith(t2.Content);
        }
        public bool HasAnyMatch()
        {
            var occupied = _gridService.GetOccupiedPositions();
            for (int i = 0; i < occupied.Count; i++)
            {
                for (int j = i + 1; j < occupied.Count; j++)
                {
                    if (CheckIsMatch(occupied[i], occupied[j]))
                    {
                        var path = CheckMatch(occupied[i], occupied[j]);
                        if (path != null && path.Count > 0)
                        {
                            return true; 
                        }
                    }
                }
            }
            return false; 
        }
    }
}