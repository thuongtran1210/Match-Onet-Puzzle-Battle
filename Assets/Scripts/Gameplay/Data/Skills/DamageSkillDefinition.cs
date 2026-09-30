using UnityEngine;
using BeastLinkBattle.Gameplay.Battle;

namespace BeastLinkBattle.Gameplay.Data.Skills
{
    [CreateAssetMenu(menuName = "Game/Skills/Damage Skill")]
    public class DamageSkillDefinition : SkillDefinition
    {
        [Header("Damage Stats")]
        public int damageAmount = 500;

        public override void Execute(BattleSimulationService simulation)
        {
            var enemies = simulation.GetAllEnemies();
            foreach (var enemy in enemies)
            {
                if (enemy.IsAlive) enemy.TakeDamage(damageAmount);
            }

            Debug.Log($"[{skillName}] Kích ho?t! Gây {damageAmount} sát thuong di?n r?ng.");
            // TODO: Instantiate(vfxPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}