using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using BeastLinkBattle.Gameplay.Data;
using DG.Tweening;

namespace BeastLinkBattle.UI.Views
{
    public class BeastIconView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _bgImage;
        [SerializeField] private GameObject _borderObj;

        private BeastDefinition _myDef;
        private Action<BeastDefinition> _onClickCallback;

        // Lưu trạng thái chọn hiện tại
        public bool IsSelected { get; private set; }

        public void Setup(BeastDefinition def, Color bgColor, Action<BeastDefinition> onClick)
        {
            _myDef = def;
            _onClickCallback = onClick;

            SetSelected(false); // Mặc định ẩn viền khi mới tạo

            if (_bgImage) _bgImage.color = bgColor;

            if (_bgImage && def.uiIcon != null)
            {
                _bgImage.sprite = def.uiIcon;
            }

            if (_text) _text.text = def.name.Substring(0, 1);

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(HandleClick);

            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }

        // Hàm để bật/tắt viền chọn từ bên ngoài (Presenter)
        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            if (_borderObj) _borderObj.SetActive(IsSelected);
        }

        private void HandleClick()
        {
            // Bật/tắt trạng thái viền ngay khi click
            SetSelected(!IsSelected);

            // Truyền sự kiện ra ngoài cho Presenter xử lý logic tiếp theo
            _onClickCallback?.Invoke(_myDef);
        }

        // Hàm riêng biệt dùng khi bạn muốn Deploy unit từ hàng chờ (ẩn icon đi)
        public void ConsumeAndHide(Action onComplete = null)
        {
            _button.interactable = false;

            // Có thể nhá sáng viền lên một chút trước khi biến mất để tạo cảm giác phản hồi tốt hơn
            if (_borderObj) _borderObj.SetActive(true);

            transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}