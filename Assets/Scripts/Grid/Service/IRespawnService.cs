using System.Collections.Generic;

namespace BeastLinkBattle.Grid.Service
{
    public interface IRespawnService
    {
        void ExecuteRespawn(RespawnRequest request);
    }
}