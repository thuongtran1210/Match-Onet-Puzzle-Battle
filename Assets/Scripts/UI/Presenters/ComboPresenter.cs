using BeastLinkBattle.Gameplay;
// Scripts/Gameplay/Presenter/ComboPresenter.cs
using BeastLinkBattle.UI.Views;
using System;

namespace BeastLinkBattle.UI.Presenters
{
    // ComboPresenter là một lớp chịu trách nhiệm kết nối giữa ComboView và IComboService. 
    // Nó lắng nghe các sự kiện từ IComboService để cập nhật UI tương ứng, như hiển thị combo hiện tại,
    // cập nhật thanh thời gian, và xử lý khi combo kết thúc. Khi được dispose, nó sẽ hủy đăng ký tất cả các sự kiện để tránh rò rỉ bộ nhớ.

    public class ComboPresenter : IDisposable
    {
        private readonly ComboView _view;
        private readonly IComboService _service;

        public ComboPresenter(ComboView view, IComboService service)
        {
            _view = view;
            _service = service;

            _service.OnComboUpdated += HandleComboUpdated;
            _service.OnTimerUpdated += HandleTimerUpdated;
            _service.OnComboEnded += HandleComboEnded;
        }

        private void HandleComboUpdated(int currentCombo)
        {
            _view.ShowComboContainer();
        }

        private void HandleTimerUpdated(float timeLeft)
        {
            float fillRatio = timeLeft / _service.MaxTimeLimit;
            _view.UpdateTimerBar(fillRatio);

            if (fillRatio <= 0.3f) 
            {
                _view.PlayWarningEffect();
            }
            else
            {
                _view.StopWarningEffect();
            }
        }

        private void HandleComboEnded()
        {
            _view.HideComboContainer();
            _view.StopWarningEffect();
        }

        public void Dispose()
        {
            _service.OnComboUpdated -= HandleComboUpdated;
            _service.OnTimerUpdated -= HandleTimerUpdated;
            _service.OnComboEnded -= HandleComboEnded;
        }
    }
}