using UnityEngine;
using BeastLinkBattle.Gameplay.Battle;

namespace BeastLinkBattle.Gameplay.Data.Skills
{
    [CreateAssetMenu(menuName = "Game/Skills/Heal Skill")]
    public class HealSkillDefinition : SkillDefinition
    {
        [Header("Heal Stats")]
        public int healAmount = 300;

        public override void Execute(BattleSimulationService simulation)
        {
            var allies = simulation.GetAllPlayerBeasts();
            foreach (var ally in allies)
            {
                if (ally.IsAlive) ally.Heal(healAmount);
            }
            Debug.Log($"[{skillName}] Kích ho?t! H?i {healAmount} máu cho toàn d?i.");
        }
    }
}