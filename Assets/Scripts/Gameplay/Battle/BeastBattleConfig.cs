using UnityEngine;
using BeastLinkBattle.Gameplay.Data;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Data.Skills;
using System;

namespace BeastLinkBattle.Gameplay.Battle
{
    [CreateAssetMenu(menuName = "Game/Battle/Beast Config")]
    public class BeastBattleConfig : ScriptableObject
    {
        [Header("Global Modifiers")]
        [Tooltip("Hệ số nhân HP theo sao (index 0 là placeholder cho 0-sao, index 1 là 1-sao, v.v.)")]
        // 1-sao: 1x | 2-sao: 3.5x | 3-sao: 10x 
        public float[] hpMultipliers = { 0f, 1f, 3.5f, 10f };
        // 1-sao: 1x | 2-sao: 2.5x | 3-sao: 6x
        public float[] dmgMultipliers = { 0f, 1f, 2.5f, 6f };
        // 1-sao: 0m | 2-sao: 0.5m | 3-sao: 1.5m
        public float[] splashRadiusByStar = { 0f, 0f, 0.5f, 1.5f };

        [Header("Formation Offsets ")]
        [Tooltip("Khoảng cách theo trục Z giữa các hàng dựa trên vai trò của thú (dương là tiến về phía trước, âm là lùi về sau)")]
        public float tankerOffset = 1.5f;     // Hàng trước
        public float assassinOffset = 0.5f;   // Hàng giữa
        public float rangerOffset = -1.0f;    // Hàng sau
        public float mageOffset = -1.5f;      // Hàng sau cùng 

        [Header("Boss Modifiers")]
        public float bossHpMultiplier = 8f;
        public float bossDamageMultiplier = 2f;

        [Header("--- ENERGY SKILLS SETUP ---")]
        public List<EnergySkillMapping> energySkills;

        [Serializable]
        public class EnergySkillMapping
        {
            public EnergyType type;
            public SkillDefinition skill;
        }

        public float GetRoleOffset(RoleType role)
        {
            switch (role)
            {
                case RoleType.Tanker: return tankerOffset;
                case RoleType.Assassin: return assassinOffset;
                case RoleType.Ranger: return rangerOffset;
                case RoleType.Mage: return mageOffset;
                default: return 0f;
            }
        }

        // HELPER
        public float GetHpMultiplier(int starLevel) => starLevel >= 0 && starLevel < hpMultipliers.Length ? hpMultipliers[starLevel] : starLevel;
        public float GetDmgMultiplier(int starLevel) => starLevel >= 0 && starLevel < dmgMultipliers.Length ? dmgMultipliers[starLevel] : starLevel;
        public float GetSplashRadius(int starLevel) => starLevel >= 0 && starLevel < splashRadiusByStar.Length ? splashRadiusByStar[starLevel] : 0f;
    }
}