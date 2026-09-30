using System;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Data; // Cần thiết cho BasePetDefinition và BeastDefinition
using BeastLinkBattle.Grid.Data;

namespace BeastLinkBattle.Services
{
    public interface IInventoryService
    {
        event Action OnInventoryUpdated;
        void Initialize();

        // READ
        List<BasePetDefinition> GetUnlockedPets();
        List<BeastDefinition> GetUnlockedBeasts();
        List<EnergyDefinition> GetUnlockedEnergies(); 

        // WRITE
        void UnlockPet(BasePetDefinition pet); 
        void UnlockBeast(BeastDefinition beast);
        void UnlockEnergy(EnergyDefinition energy); 

        void SaveInventory();
    }
}