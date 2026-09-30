using UnityEngine;
using UnityEngine.UI;
using System;
using BeastLinkBattle.Gameplay.Data;
using DG.Tweening;

namespace BeastLinkBattle.UI.Views
{
    public class BeastToggleView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _button;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _bgImage;
        [SerializeField] private GameObject _selectedHighlight; // Khung viền báo hiệu đang được chọn
        public BeastDefinition MyDef => _myDef;
        private BeastDefinition _myDef;
        private Action<BeastDefinition> _onSelect;
        private Action<BeastDefinition> _onDeselect;
        private bool _isSelected = false;

        public void Setup(BeastDefinition def, Action<BeastDefinition> onSelect, Action<BeastDefinition> onDeselect)
        {
            _myDef = def;
            _onSelect = onSelect;
            _onDeselect = onDeselect;
            _isSelected = false;

            if (_iconImage && def.uiIcon != null) _iconImage.sprite = def.uiIcon;
            if (_bgImage) _bgImage.color = def.uiColor;
            if (_selectedHighlight) _selectedHighlight.SetActive(false);

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(HandleToggle);
        }

        private void HandleToggle()
        {
            // Hiệu ứng bấm nút
            transform.DOKill();
            transform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.15f);

            _isSelected = !_isSelected;

            if (_selectedHighlight) _selectedHighlight.SetActive(_isSelected);

            if (_isSelected)
            {
                _onSelect?.Invoke(_myDef);
            }
            else
            {
                _onDeselect?.Invoke(_myDef);
            }
        }

        // Hàm hỗ trợ để ép tắt state nếu bị quá giới hạn từ Presenter dội về
        public void ForceDeselect()
        {
            _isSelected = false;
            if (_selectedHighlight) _selectedHighlight.SetActive(false);
        }
        public void ForceSelectWithoutNotify()
        {
            _isSelected = true;
            if (_selectedHighlight) _selectedHighlight.SetActive(true);
        }

    }
}