using System;
using BeastLinkBattle.Core;
using BeastLinkBattle.Core.Coordinators;
using BeastLinkBattle.Gameplay;
using BeastLinkBattle.UI.Views;

namespace BeastLinkBattle.UI.Presenters
{

    // GameOverPresenter là một lớp chịu trách nhiệm kết nối giữa GameOverView và GameBootstrapper cùng với GameGameplayMediator.
    // Nó lắng nghe các sự kiện từ GameGameplayMediator để hiển thị kết quả khi trò chơi kết thúc,
    //  và xử lý khi người chơi nhấn nút "Retry". Khi được dispose, nó sẽ hủy đăng ký tất cả các sự kiện để tránh rò rỉ bộ nhớ.          
    public class GameOverPresenter : IDisposable
    {
        private readonly GameOverView _view;
        private readonly GameBootstrapper _bootstrapper;
        private readonly IBattleService _battleService;
        private readonly GameStateCoordinator _stateCoordinator;

        public GameOverPresenter(GameOverView view, GameBootstrapper bootstrapper, GameStateCoordinator stateCoordinator)
        {
            _view = view;
            _bootstrapper = bootstrapper;
            _stateCoordinator = stateCoordinator;
            _view.OnRetryClicked += HandleRetry;
            _stateCoordinator.OnGameOverUIRequested += HandleGameOverUI;
        }

        private void HandleGameOverUI(bool isWin, int score)
        {
            _view.DisplayResult(isWin, score);
        }

        private void HandleRetry()
        {
            _view.Hide();
        }

        private void HandleHome()
        {
        }

        public void Dispose()
        {
            _view.OnRetryClicked -= HandleRetry;

            if (_stateCoordinator != null)
                _stateCoordinator.OnGameOverUIRequested -= HandleGameOverUI;
        }
    }
}
