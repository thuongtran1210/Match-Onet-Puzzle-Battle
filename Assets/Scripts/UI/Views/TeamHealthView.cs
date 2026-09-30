using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace BeastLinkBattle.UI.Views
{

    // View này sẽ hiển thị trong trận đấu, ở góc trên cùng của màn hình, để thể hiện lượng HP còn lại của cả đội hình người chơi và kẻ địch.
    public class TeamHealthView : MonoBehaviour
    {
        [Header("UI References - Player")]
        [SerializeField] private Image _playerHealthBar;
        [SerializeField] private TextMeshProUGUI _playerHealthText;

        [Header("UI References - Enemy")]
        [SerializeField] private Image _enemyHealthBar;
        [SerializeField] private TextMeshProUGUI _enemyHealthText;



        public void UpdatePlayerHealth(int currentHp, int maxHp)
        {
            UpdateBar(_playerHealthBar, _playerHealthText, currentHp, maxHp);
        }

        public void UpdateEnemyHealth(int currentHp, int maxHp)
        {
            UpdateBar(_enemyHealthBar, _enemyHealthText, currentHp, maxHp);
        }

        private void UpdateBar(Image bar, TextMeshProUGUI text, int current, int max)
        {
            float ratio = max > 0 ? (float)current / max : 0;
            if (bar != null) bar.DOFillAmount(ratio, 0.3f);
            if (text != null) text.text = $"{current}/{max}";
        }
    }
}