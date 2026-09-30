using System.Collections.Generic;
using BeastLinkBattle.Grid.Data;

namespace BeastLinkBattle.Services 
{
    [System.Serializable]
    public class DeckSaveData
    {
        public string LeaderPetId;
        public List<string> SelectedBeastIds;
        public List<string> SelectedEnergyIds;
    }
}