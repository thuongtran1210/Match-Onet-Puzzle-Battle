using UnityEngine;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Battle.Models;
using BeastLinkBattle.Gameplay.Data;

namespace BeastLinkBattle.Gameplay.Battle.Behaviors
{
    [CreateAssetMenu(menuName = "Game/Battle Behaviors/Melee Attack")]
    public class MeleeAttackBehavior : AttackBehavior
    {
        public override void ExecuteAttack(BaseEntityModel attacker, BaseEntityModel primaryTarget, List<BaseEntityModel> allDefenders)
        {
            int finalDamage = attacker.Damage;

            if (attacker.Element == ElementType.Water && primaryTarget.Element == ElementType.Fire)
            {
                finalDamage = Mathf.RoundToInt(finalDamage * 1.5f);
                Debug.Log("Sát thuong kh?c h?: Water -> Fire!");
            }

            primaryTarget.TakeDamage(finalDamage);
        }
    }
}