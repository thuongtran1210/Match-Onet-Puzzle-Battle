// Scripts/Gameplay/Data/EnergyDefinition.cs
using UnityEngine;
using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Gameplay.Data.Skills;

namespace BeastLinkBattle.Gameplay.Data
{
    [CreateAssetMenu(menuName = "Game/Data/Energy Definition", fileName = "Energy_")]
    public class EnergyDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string energyId;
        public EnergyType type; 

        [Header("UI Presentation")]
        public string displayName;
        public Sprite uiIcon;
        public Color uiColor = Color.white;

        [Header("Effect Stats (Base)")]
        public float baseEffectValue; // Vd: Damage = 10, Heal = 15, Speed = 1.5
        public float duration; // Dành cho các buff như Speed/Slow

        [Header("Prefabs")]
        public GameObject gridPrefab;

        [Header("Skill Settings")]
        public SkillDefinition grantedSkill;

        // Sau này muốn nâng cấp, có thể biến các stat này thành mảng hoặc object chứa theo Level
        // public EnergyLevelData[] levels;
    }
}