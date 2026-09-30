using UnityEngine;

namespace BeastLinkBattle.Grid.Data.Policies
{
    [CreateAssetMenu(menuName = "Game/Policies/Percentage Respawn")]
    public class PercentageRespawnPolicy : RespawnPolicy
    {
        [Range(0f, 1f)]
        [Tooltip("Tỉ lệ tối đa của ô có chứa content so với tổng số ô chơi được, dưới ngưỡng này sẽ kích hoạt respawn")]
        public float threshold = 0.3f;

        [Tooltip("Khoảng thời gian delay giữa các lần respawn liên tiếp khi điều kiện vẫn được đáp ứng")]
        public float spawnDelay = 0.1f;

        public override bool ShouldTriggerRespawn(int totalPlayableTiles, int emptyTilesCount)
        {
            if (totalPlayableTiles == 0) return false;

            int currentOccupied = totalPlayableTiles - emptyTilesCount;
            float currentPercent = (float)currentOccupied / totalPlayableTiles;
            return currentPercent <= threshold;
        }

        public override float GetSequentialDelay() => spawnDelay;
    }
}