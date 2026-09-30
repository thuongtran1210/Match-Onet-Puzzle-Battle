using System.Collections.Generic;
using BeastLinkBattle.Grid.Data;

namespace BeastLinkBattle.Services
{
    [System.Serializable]
    public class InventorySaveData
    {
        public List<string> UnlockedPetIds;
        public List<string> UnlockedBeastIds;
        public List<string> UnlockedEnergyIds;
    }
}