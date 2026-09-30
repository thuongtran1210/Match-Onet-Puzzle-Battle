using BeastLinkBattle.Grid.Service.Rules;
using BeastLinkBattle.Grid.Service;
using System.Collections.Generic;
using UnityEngine;


// MagicWandMatchRule là một quy tắc ghép đôi đặc biệt cho phép người chơi sử dụng một "Magic Wand" để ghép đôi bất kỳ hai ô nào trên lưới,
// bỏ qua các quy tắc thông thường về đường đi và chướng ngại vật.
public class MagicWandMatchRule : IMatchRule
{
    private bool _isWandActive; 

    public List<Vector2Int> FindPath(Vector2Int p1, Vector2Int p2, IGridService gridService)
    {
        if (!_isWandActive) return null;
        return new List<Vector2Int> { p1, p2 }; 
    }
}