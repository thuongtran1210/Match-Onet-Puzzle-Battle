using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Gameplay;
// Scripts/Gameplay/Presenter/EnergyPresenter.cs
using BeastLinkBattle.UI.Views;
using System;
using BeastLinkBattle.Gameplay.Data;
using System.Collections.Generic;

namespace BeastLinkBattle.UI.Presenters
{
    //  EnergyPresenter là một lớp chịu trách nhiệm kết nối giữa EnergyView và IEnergyService.
    // Nó lắng nghe các sự kiện từ IEnergyService để cập nhật UI tương ứng, như hiển thị lượng năng lượng hiện tại,
    // hiển thị trạng thái đầy năng lượng, và xử lý khi người chơi cố gắng sử dụng kỹ năng. 
    // Khi được dispose, nó sẽ hủy đăng ký tất cả các sự kiện để tránh rò rỉ bộ nhớ.
    public class EnergyPresenter : IDisposable
    {
        private readonly EnergyView _view;
        private readonly IEnergyService _energyService;
        private readonly IBattleService _battleService;

        public EnergyPresenter(EnergyView view, IEnergyService energyService, IBattleService battleService)
        {
            _view = view;
            _energyService = energyService;
            _battleService = battleService;
            _energyService.OnEnergyChanged += _view.UpdateEnergyBar;
            _energyService.OnEnergyFull += _view.ShowFullState;

            _view.OnSkillButtonClicked += RequestSkillCast;
        }
        public void StartBattle(List<EnergyDefinition> activeEnergies)
        {
            // 1. Yêu cầu View tạo các nút dựa trên danh sách Energy trong Deck
            _view.Initialize(activeEnergies);

            // 2. Hiện View lên (nếu đang ẩn)
            _view.gameObject.SetActive(true);
        }

        private void RequestSkillCast(EnergyDefinition def)
        {
            if (_energyService.TryConsumeEnergyForSkill(def))
            {
                _battleService.ExecuteSkill(def.grantedSkill);
            }
        }

        public void Dispose()
        {
            _energyService.OnEnergyChanged -= _view.UpdateEnergyBar;
            _energyService.OnEnergyFull -= _view.ShowFullState;
            _view.OnSkillButtonClicked -= RequestSkillCast;
        }
    }
}