using System.Collections.Generic;
using UnityEngine;
namespace  BeastLinkBattle.Grid.Service
{
    public interface IMatchService
    {
        List<Vector2Int> CheckMatch(Vector2Int posA, Vector2Int posB);
        bool HasAnyMatch();
    }
}
