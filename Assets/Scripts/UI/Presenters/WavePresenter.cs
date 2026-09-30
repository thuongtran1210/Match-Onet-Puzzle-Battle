// Scripts/Gameplay/Presenter/WavePresenter.cs
using BeastLinkBattle.Gameplay.Battle;
using BeastLinkBattle.UI.Views;
using System;

namespace BeastLinkBattle.UI.Presenters
{

    // WavePresenter là một lớp chịu trách nhiệm quản lý giao diện hiển thị thông tin về wave hiện tại trong trận đấu.
    // Nó lắng nghe sự kiện từ IEnemySpawnerService để cập nhật số wave hiện tại và tổng số wave,
    // cũng như hiển thị hiệu ứng cảnh báo khi một wave boss sắp xuất hiện.
    // Khi số wave thay đổi, nó sẽ cập nhật văn bản hiển thị trên WaveView để phản ánh thông tin mới.
    public class WavePresenter : IDisposable
    {
        private readonly WaveView _view;
        private readonly IEnemySpawnerService _spawnerService;

        public WavePresenter(WaveView view, IEnemySpawnerService spawnerService)
        {
            _view = view;
            _spawnerService = spawnerService;

            _spawnerService.OnWaveChanged += HandleWaveChanged;
            _spawnerService.OnWaveWarning += _view.PlayBossWarningEffect;
        }

        private void HandleWaveChanged(int currentWave, int totalWaves)
        {
            string waveText = $"WAVE {currentWave}/{totalWaves}";
            _view.UpdateWaveText(waveText);
        }

        public void Dispose()
        {
            _spawnerService.OnWaveChanged -= HandleWaveChanged;
            _spawnerService.OnWaveWarning -= _view.PlayBossWarningEffect;
        }
    }
}