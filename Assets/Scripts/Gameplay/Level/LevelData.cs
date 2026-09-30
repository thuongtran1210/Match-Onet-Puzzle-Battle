using UnityEngine;
using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Grid.Data.Policies;
using BeastLinkBattle.Gameplay.Battle.Data;
using System.Collections.Generic;

namespace BeastLinkBattle.Gameplay.Level
{
    [CreateAssetMenu(menuName = "Game/Level Data", fileName = "Level_")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Info")]
        public int levelId;
        public string levelName;

        [Header("Grid Configurations")]
        public GridShape shape;

        [Tooltip("Quy định điều kiện và tốc độ rớt block của màn chơi này")]
        public RespawnPolicy respawnPolicy;

        [Header("Battle Configurations")]
        [Tooltip("Danh sách các wave quái sinh ra trong level này")]
        public List<WaveData> waves;
    }
}