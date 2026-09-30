// File: Scripts/Gameplay/Data/WaveData.cs
using UnityEngine;
using System.Collections.Generic;

namespace BeastLinkBattle.Gameplay.Battle.Data
{
    [CreateAssetMenu(menuName = "Game/Battle/Wave Data")]
    public class WaveData : ScriptableObject
    {
        public string waveName = "Wave 1";

        [Tooltip("Có đợi cho đến khi tất cả kẻ địch của Wave trước bị tiêu diệt trước khi bắt đầu Wave này không? Nếu không, Wave tiếp theo sẽ bắt đầu sau một khoảng thời gian nhất định.")]
        public bool waitForClear = false;

        [Tooltip("Nếu 'waitForClear' là false, đây là khoảng thời gian (tính bằng giây) trước khi Wave tiếp theo bắt đầu sau khi Wave hiện tại bắt đầu.")]
        public float timeToNextWave = 10f;

        [Tooltip("Danh sách cấu hình sinh kẻ địch cho Wave này. Mỗi mục trong danh sách đại diện cho một loại kẻ địch sẽ được sinh ra, số lượng, khoảng thời gian giữa các lần sinh và chỉ số sao của chúng.")]
        public List<EnemySpawnConfig> enemies;
    }
}