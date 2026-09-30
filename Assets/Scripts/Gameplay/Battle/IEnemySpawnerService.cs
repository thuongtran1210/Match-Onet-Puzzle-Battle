using BeastLinkBattle.Gameplay.Battle.Data;
using System;
using System.Collections.Generic;

namespace BeastLinkBattle.Gameplay.Battle
{
    // Interface cho EnemySpawnerService, nơi sẽ xử lý logic sinh quái của trận đấu, bao gồm:
    // - Quản lý các wave quái, bao gồm loại quái, số lượng và thời gian xuất hiện
    // - Cung cấp sự kiện để thông báo cho hệ thống khi cần sinh quái
    public interface IEnemySpawnerService
    {
        event Action<EnemyDefinition, int, bool> OnEnemySpawnRequested;

        event Action OnWaveWarning;
        event Action<int, int> OnWaveChanged;
        void StartLevel(List<WaveData> waves);
        void Tick(float deltaTime);
        void RegisterEnemyDeath();
        bool IsReadyForNextWave();
        bool TryAdvanceToNextWave();
    }
}