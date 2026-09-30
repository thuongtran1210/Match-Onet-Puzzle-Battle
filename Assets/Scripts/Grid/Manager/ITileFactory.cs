using BeastLinkBattle.Grid.Data;
using UnityEngine;
namespace BeastLinkBattle.Grid.Manager
{
    public enum SpawnMode
    {
        Mixed,
        BeastOnly,
        EnergyOnly
    }
    public interface ITileFactory
    {
        void SetSpawnMode(SpawnMode mode);
        ITileContent CreateRandomContent();
        ITileContent[] CreateRandomContentPair();
    }
}
