using UnityEngine;
using System;
// Xóa dòng using System.Collections.Generic; n?u không c?n thi?t

namespace BeastLinkBattle.Gameplay.Battle.Data
{
    [Serializable]
    public class EnemySpawnConfig
    {
        public EnemyDefinition definition;

        [Tooltip("Số lượng kẻ địch cùng loại sẽ sinh ra trong một đợt")]
        public int count = 1;
        [Tooltip("Khoảng thời gian (tính bằng giây) giữa các lần sinh kẻ địch cùng loại trong một đợt")]
        public float delayBetweenSpawns = 0.5f;
        [Tooltip("Chỉ số sao của kẻ địch, ảnh hưởng đến sức mạnh và phần thưởng khi bị đánh bại")]
        public int starLevel = 1;

        [Header("Mini-Boss Settings")]
        [Tooltip("Ðánh dấu nếu kẻ địch này là Mini-Boss, sẽ có chỉ số mạnh hơn và phần thưởng lớn hơn")]
        public bool isMiniBoss = false;
    }
}