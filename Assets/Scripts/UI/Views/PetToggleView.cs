using UnityEngine;
using UnityEngine.UI;
using System;
using BeastLinkBattle.Gameplay.Data;
using DG.Tweening;

namespace BeastLinkBattle.UI.Views
{
    public class PetToggleView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _button;
        [SerializeField] private Image _iconImage;
        [SerializeField] private GameObject _selectedHighlight; // Khung viền báo hiệu Pet đang được chọn làm Leader

        public BasePetDefinition MyDef => _myDef;
        private BasePetDefinition _myDef;
        private Action<BasePetDefinition> _onSelect;
        private Action<BasePetDefinition> _onDeselect;
        private bool _isSelected = false;

        public void Setup(BasePetDefinition def, Action<BasePetDefinition> onSelect = null, Action<BasePetDefinition> onDeselect = null)
        {
            _myDef = def;
            _onSelect = onSelect;
            _onDeselect = onDeselect;
            _isSelected = false;

            if (_iconImage && def.uiIcon != null)
            {
                _iconImage.sprite = def.uiIcon;
            }

            if (_selectedHighlight) _selectedHighlight.SetActive(false);

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(HandleToggle);
        }

        private void HandleToggle()
        {
            // Hiệu ứng bấm nút giống BeastToggleView
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

        // Ép tắt trạng thái (dùng khi chọn một Pet khác làm Leader)
        public void ForceDeselect()
        {
            _isSelected = false;
            if (_selectedHighlight) _selectedHighlight.SetActive(false);
        }

        // Bật trạng thái khi load SaveData ban đầu mà không bắn sự kiện
        public void ForceSelectWithoutNotify()
        {
            _isSelected = true;
            if (_selectedHighlight) _selectedHighlight.SetActive(true);
        }

        // Tắt trạng thái mà không bắn sự kiện
        public void ForceDeselectWithoutNotify()
        {
            _isSelected = false;
            if (_selectedHighlight) _selectedHighlight.SetActive(false);
        }
    }
}