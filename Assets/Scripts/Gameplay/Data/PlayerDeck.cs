using System.Collections.Generic;
using BeastLinkBattle.Grid.Data;

namespace BeastLinkBattle.Gameplay.Data
{
    public class PlayerDeck
    {
        public BasePetDefinition LeaderPet { get; private set; }
        public List<BeastDefinition> SelectedBeasts { get; private set; }

        public List<EnergyDefinition> SelectedEnergies { get; private set; }

        public PlayerDeck(BasePetDefinition leader, List<BeastDefinition> beasts, List<EnergyDefinition> energies)
        {
            LeaderPet = leader;
            SelectedBeasts = beasts;
            SelectedEnergies = energies; 
        }
    }
}