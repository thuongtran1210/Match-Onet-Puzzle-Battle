using System;
using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Gameplay.Data;

namespace BeastLinkBattle.Gameplay
{
    public interface IEnergyService
    {
        event Action<EnergyDefinition, int, int> OnEnergyChanged;
        event Action<EnergyDefinition> OnEnergyFull;

        void ProcessMatchedContent(ITileContent content);
        bool TryConsumeEnergyForSkill(EnergyDefinition def);
    }
}