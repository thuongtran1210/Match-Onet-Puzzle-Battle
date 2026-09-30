using BeastLinkBattle.Gameplay;
using System;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Battle;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.UI.Views;
using BeastLinkBattle.Core;

namespace BeastLinkBattle.UI.Presenters
{
    public class BattleQueuePresenter : IDisposable
    {
        private readonly BattleQueueView _view;
        private readonly IBattleService _battleService;
        private readonly GameBootstrapper _bootstrapper;

        // Bom (Inject) View, Service và Bootstrapper vào Presenter
        public BattleQueuePresenter(BattleQueueView view, IBattleService battleService, GameBootstrapper bootstrapper)
        {
            _view = view;
            _battleService = battleService;
            _bootstrapper = bootstrapper;

            // 1. LẮNG NGHE THAY ĐỔI TRONG HÀNG CHỜ: Cập nhật View khi có thay đổi trong hàng chờ
            _battleService.OnQueueUpdated += HandleQueueUpdated;

            // 2. LẮNG NGHE SỰ KIỆN TỪ VIEW: Xử lý khi người chơi tương tác với UI
            _view.OnStartBattleClicked += HandleStartBattleClicked;
            _view.OnBeastIconClicked += HandleBeastIconClicked;
        }

        /// <summary>
        /// Xữ lý cập nhật hàng chờ: Khi có thay đổi trong hàng chờ, Presenter sẽ nhận được thông báo và cập nhật View tương ứng.
        /// </summary>
        private void HandleQueueUpdated(Dictionary<BeastDefinition, int> currentQueue)
        {
            _view.UpdateQueueDisplay(currentQueue);
        }

        /// <summary>
        /// Xữ lý khi người chơi bấm nút "Start Battle": Presenter sẽ gọi hàm trong Service để bắt đầu trận đấu, dựa trên hàng chờ hiện tại.
        /// </summary>
        private void HandleStartBattleClicked()
        {
            _bootstrapper.ResolveMatchQueue();
        }

        /// <summary>
        /// Xữ lý khi người chơi bấm vào biểu tượng thú cưng trong hàng chờ.
        /// </summary>
        ///  Presenter sẽ kiểm tra xem thú cưng đó còn trong hàng chờ hay không,
 
        private void HandleBeastIconClicked(BeastDefinition clickedBeastDef)
        {
            // Ki?m tra xem thú này còn trong hàng ch? không tru?c khi th?
            if (_battleService.HasBeastInQueue(clickedBeastDef))
            {
                // G?i hàm sinh ra m?t thú duy nh?t
                _bootstrapper.SpawnSingleBeast(clickedBeastDef);
            }
        }

        public void Dispose()
        {
            _battleService.OnQueueUpdated -= HandleQueueUpdated;
            _view.OnStartBattleClicked -= HandleStartBattleClicked;
            _view.OnBeastIconClicked -= HandleBeastIconClicked;
        }
    }
}