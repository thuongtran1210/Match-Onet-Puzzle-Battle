using UnityEngine;

namespace BeastLinkBattle.Grid.Data.Policies
{
    public abstract class RespawnPolicy : ScriptableObject
    {
        public abstract bool ShouldTriggerRespawn(int totalPlayableTiles, int emptyTilesCount);

        public abstract float GetSequentialDelay();
    }
}