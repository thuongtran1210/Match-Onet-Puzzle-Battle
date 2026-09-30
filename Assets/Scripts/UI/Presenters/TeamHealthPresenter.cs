using BeastLinkBattle.Gameplay;
using BeastLinkBattle.Gameplay.Battle.Models;
using BeastLinkBattle.UI.Views;
using System.Collections.Generic;

namespace BeastLinkBattle.UI.Presenters
{

    // TeamHealthPresenter là một lớp chịu trách nhiệm quản lý và cập nhật giao diện hiển thị sức khỏe tổng thể của đội hình người chơi và kẻ thù trong trận đấu.
    // Nó lắng nghe sự kiện khi các thú cưng của người chơi và kẻ thù được tạo ra, 
    // và đăng ký các sự kiện liên quan đến việc nhận sát thương, hồi phục, 
    // và chết của chúng để tính toán và cập nhật tổng HP hiện tại và tối đa cho cả hai đội trên giao diện người dùng thông qua TeamHealthView.
    public class TeamHealthPresenter
    {
        private readonly TeamHealthView _view;
        private readonly IBattleService _battleService;

        private List<BaseEntityModel> _playerModels = new List<BaseEntityModel>();
        private List<BaseEntityModel> _enemyModels = new List<BaseEntityModel>();

        // Inject View và Service vào Presenter
        public TeamHealthPresenter(TeamHealthView view, IBattleService battleService)
        {
            _view = view;
            _battleService = battleService;
            _battleService.OnPlayerBeastCreated += RegisterPlayerBeast;
            _battleService.OnEnemyCreated += RegisterEnemyBeast;
        }

        private void RegisterPlayerBeast(PlayerBeastModel model)
        {
            _playerModels.Add(model);
            model.OnTakeDamage += _ => CalculatePlayerHealth();
            model.OnHeal += _ => CalculatePlayerHealth();
            model.OnDie += () => {
                _playerModels.Remove(model);
                CalculatePlayerHealth();
            };
            CalculatePlayerHealth();
        }

        private void RegisterEnemyBeast(EnemyModel model)
        {
            _enemyModels.Add(model);
            model.OnTakeDamage += _ => CalculateEnemyHealth();
            model.OnHeal += _ => CalculateEnemyHealth();
            model.OnDie += () => {
                _enemyModels.Remove(model);
                CalculateEnemyHealth();
            };
            CalculateEnemyHealth();
        }

        private void CalculatePlayerHealth()
        {
            int totalCurrent = 0, totalMax = 0;
            foreach (var m in _playerModels)
            {
                if (m.IsAlive)
                {
                    totalCurrent += m.CurrentHP;
                    totalMax += m.MaxHP;
                }
            }
            _view.UpdatePlayerHealth(totalCurrent, totalMax);
        }

        private void CalculateEnemyHealth()
        {
            int totalCurrent = 0, totalMax = 0;
            foreach (var m in _enemyModels)
            {
                if (m.IsAlive)
                {
                    totalCurrent += m.CurrentHP;
                    totalMax += m.MaxHP;
                }
            }
            _view.UpdateEnemyHealth(totalCurrent, totalMax);
        }

        public void Dispose()
        {
            _battleService.OnPlayerBeastCreated -= RegisterPlayerBeast;
            _battleService.OnEnemyCreated -= RegisterEnemyBeast;
        }
    }
}