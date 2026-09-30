using UnityEngine;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Battle.Models;
using BeastLinkBattle.Gameplay.Battle.Effects;
using BeastLinkBattle.Gameplay.Data;

namespace BeastLinkBattle.Gameplay.Battle.Behaviors
{
    [CreateAssetMenu(menuName = "Game/Battle Behaviors/Splash Attack")]
    public class SplashAttackBehavior : AttackBehavior
    {
        [Tooltip("Bán kính")]
        public float splashRadius = 1.5f;

        [Tooltip("Sát thương lan tỏa (tỉ lệ phần trăm so với sát thương chính)")]
        public float splashDamageRatio = 0.5f;

        public override void ExecuteAttack(BaseEntityModel attacker, BaseEntityModel primaryTarget, List<BaseEntityModel> allDefenders)
        {
            primaryTarget.TakeDamage(attacker.Damage);

            float sqrSplash = splashRadius * splashRadius;
            Vector3 impactPoint = primaryTarget.Position;
            int splashDamage = Mathf.RoundToInt(attacker.Damage * splashDamageRatio);

            foreach (var def in allDefenders)
            {
                if (def != primaryTarget && def.IsAlive)
                {
                    float sqrDist = (impactPoint - def.Position).sqrMagnitude;
                    if (sqrDist <= sqrSplash)
                    {
                        def.TakeDamage(splashDamage);

                        if (attacker.Element == ElementType.Fire)
                        {
                            int burnDmg = Mathf.Max(1, attacker.Damage / 10);
                            def.AddEffect(new BurnEffect(3.0f, burnDmg, 1.0f));
                        }
                    }
                }
            }
        }
    }
}