
using UnityEngine;
using System.Collections.Generic;
using System;
using BeastLinkBattle.Gameplay.Data; 

namespace BeastLinkBattle.Gameplay
{
    [CreateAssetMenu(menuName = "Game/Gameplay/Energy Config")]
    public class EnergyConfig : ScriptableObject
    {
        [Tooltip(" Số năng lượng nhận được khi có một match thành công")]
        public int energyPerMatch = 10;

        [Tooltip(" Số năng lượng tối đa ")]
        public List<EnergyLimit> energyLimits;

        [Serializable]
        public class EnergyLimit
        {
            public EnergyDefinition definition;
            public int maxEnergy = 100;
        }
        public int GetMaxEnergy(EnergyDefinition def)
        {
            // So sánh hai ScriptableObject với nhau
            var limit = energyLimits.Find(x => x.definition == def);
            return limit != null ? limit.maxEnergy : 100; // Default là 100
        }
    }
}