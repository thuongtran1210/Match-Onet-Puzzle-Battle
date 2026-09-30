using UnityEngine;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Battle.Models;
using BeastLinkBattle.Gameplay.Battle.Entities;
using BeastLinkBattle.Core;

namespace BeastLinkBattle.Gameplay.Battle.Behaviors
{
    [CreateAssetMenu(menuName = "Game/Battle Behaviors/Projectile Attack")]
    public class ProjectileAttackBehavior : AttackBehavior
    {
        [Header("Projectile Settings")]
        [Tooltip("Prefab của đạn bắn ra")]
        public ProjectileEntity projectilePrefab;

        [Tooltip("Tốc độ bay của đạn (Đơn vị/giây)")]
        public float flightSpeed = 8f;

        public override void ExecuteAttack(BaseEntityModel attacker, BaseEntityModel primaryTarget, List<BaseEntityModel> allDefenders)
        {
            if (projectilePrefab != null)
            {
               
                GameObject projObj = PoolManager.Instance.Get(projectilePrefab.gameObject, attacker.Position, Quaternion.identity);
                ProjectileEntity projectile = projObj.GetComponent<ProjectileEntity>();

                int finalDamage = attacker.Damage;
                projectile.Setup(primaryTarget, finalDamage, flightSpeed);
            }
            else
            {
                Debug.LogWarning($"[ProjectileAttackBehavior] Chua gán Projectile Prefab cho Behavior c?a {attacker.Role}!");
                primaryTarget.TakeDamage(attacker.Damage);
            }
        }
    }
}