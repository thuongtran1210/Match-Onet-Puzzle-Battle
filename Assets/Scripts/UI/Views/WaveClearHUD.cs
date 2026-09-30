using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

namespace BeastLinkBattle.UI.Views
{
    public class WaveClearHUD : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private CanvasGroup _canvasGroup; 
        [SerializeField] private RectTransform _contentBox;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _statsText;
        [SerializeField] private Button _nextButton;

        private Action _onNextClicked;

        private void Awake()
        {
            _nextButton.onClick.AddListener(HandleNextClicked);
        }

        // Hiển thị HUD với thông tin về wave vừa hoàn thành và số thú cưng còn sống sót.
        public void Show(int clearedWaveIndex, int survivedBeasts, Action onNextClicked)
        {
            _onNextClicked = onNextClicked;

            _titleText.text = $"WAVE {clearedWaveIndex} CLEARED!";
         


            _canvasGroup.alpha = 0f;
            _contentBox.localScale = Vector3.zero;

            _canvasGroup.DOFade(1f, 0.3f).SetUpdate(true);
            _contentBox.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        // Ẩn HUD và gọi callback khi người chơi bấm nút Next
        private void HandleNextClicked()
        {
            _nextButton.interactable = false;

            _contentBox.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).SetUpdate(true);
            _canvasGroup.DOFade(0f, 0.2f).SetUpdate(true).OnComplete(() =>
            {

                _nextButton.interactable = true;
                _onNextClicked?.Invoke(); 
            });
        }

        private void OnDestroy()
        {
            _contentBox.DOKill();
            _canvasGroup.DOKill();
        }
    }
}