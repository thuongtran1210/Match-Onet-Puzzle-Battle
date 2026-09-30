using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeastLinkBattle.UI.Views
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Text _scoreText;

        public event Action OnRetryClicked;
        public event Action OnHomeClicked;

        private void Awake()
        {
            _retryButton.onClick.AddListener(() => OnRetryClicked?.Invoke());
            _homeButton.onClick.AddListener(() => OnHomeClicked?.Invoke());
        }

        public void DisplayResult(bool isWin, int score)
        {
            _scoreText.text = $"Diem cua ban: {score}";
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
