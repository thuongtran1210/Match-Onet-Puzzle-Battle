using UnityEngine;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Battle.Models;

namespace BeastLinkBattle.Gameplay.Battle.Behaviors
{
    public abstract class AttackBehavior : ScriptableObject
    {
        public abstract void ExecuteAttack(BaseEntityModel attacker, BaseEntityModel primaryTarget, List<BaseEntityModel> allDefenders);
    }
}