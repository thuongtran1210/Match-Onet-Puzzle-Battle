using UnityEngine;
using BeastLinkBattle.Gameplay.Battle; // Ð? dùng BattleSimulationService

namespace BeastLinkBattle.Gameplay.Data.Skills
{
    public abstract class SkillDefinition : ScriptableObject
    {
        [Header("Skill Identity")]
        public string skillName;
        public Sprite icon;
        public GameObject vfxPrefab;

        [TextArea]
        public string description;

        // Hàm abstract bu?c m?i skill con ph?i t? d?nh nghia logic riêng c?a mình
        public abstract void Execute(BattleSimulationService simulation);
    }
}