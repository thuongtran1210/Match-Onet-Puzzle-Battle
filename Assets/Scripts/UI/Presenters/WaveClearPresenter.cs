using BeastLinkBattle.UI;
using System;
using BeastLinkBattle.Core;
using BeastLinkBattle.UI.Views;
using BeastLinkBattle.Core.Coordinators;

namespace BeastLinkBattle.UI.Presenters
{

    // WaveClearPresenter là một lớp chịu trách nhiệm quản lý giao diện hiển thị thông tin khi người chơi hoàn thành một wave trong trận đấu.
    // Nó lắng nghe sự kiện từ GameGameplayMediator để biết khi nào cần hiển
    // thị thông tin về wave đã hoàn thành, bao gồm số wave và số lượng thú cưng sống sót.
    public class WaveClearPresenter : IDisposable
    {
        private readonly WaveClearView _view;
        private readonly GameBootstrapper _bootstrapper;
        private readonly GameStateCoordinator _stateCoordinator;
        private readonly IUIFlowService _uiFlow;
        private Action _proceedCallback;

        public WaveClearPresenter(WaveClearView view, GameBootstrapper bootstrapper, GameStateCoordinator stateCoordinator, IUIFlowService uiFlow)
        {
            _view = view;
            _bootstrapper = bootstrapper;
            _stateCoordinator = stateCoordinator;
            _uiFlow = uiFlow;

            if (_view != null)
            {
                _view.OnNextWaveClicked += HandleNextWaveClicked;
            }

            if (_stateCoordinator != null)
            {
                _stateCoordinator.OnWaveTransitionUIRequested += HandleWaveTransition;
            }
        }

        private void HandleWaveTransition(int waveIndex, int survivedCount, Action proceedCallback)
        {
            _proceedCallback = proceedCallback;
            _view?.DisplayWaveClearInfo(waveIndex, survivedCount);
        }

        private void HandleNextWaveClicked()
        {
            _uiFlow.HidePanel(PanelType.WaveTransition);
            _proceedCallback?.Invoke();
            _proceedCallback = null;
        }

        public void Dispose()
        {
            if (_view != null)
            {
                _view.OnNextWaveClicked -= HandleNextWaveClicked;
            }

            if (_stateCoordinator != null)
                _stateCoordinator.OnWaveTransitionUIRequested -= HandleWaveTransition;
        }
    }
}
