// Scripts/UI/Views/EnergyToggleView.cs
using UnityEngine;
using UnityEngine.UI;
using System;
using BeastLinkBattle.Gameplay.Data; // Đảm bảo có namespace này
using DG.Tweening;

namespace BeastLinkBattle.UI.Views
{
    public class EnergyToggleView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _button;
        [SerializeField] private Image _iconImage;
        [SerializeField] private GameObject _selectedHighlight;

        public EnergyDefinition definition;
        private Action<EnergyDefinition> _onSelect;
        private Action<EnergyDefinition> _onDeselect;
        private bool _isSelected = false;

        public EnergyDefinition MyType => definition;

        // Cập nhật tham số Setup
        public void Setup(EnergyDefinition def, Action<EnergyDefinition> onSelect = null, Action<EnergyDefinition> onDeselect = null)
        {
            this.definition = def;
            this._onSelect = onSelect;
            this._onDeselect = onDeselect;

            if (_iconImage != null && def != null)
            {
                _iconImage.sprite = def.uiIcon;
            }

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(HandleToggle);
        }

        private void HandleToggle()
        {
            transform.DOKill();
            transform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.15f);

            _isSelected = !_isSelected;

            if (_selectedHighlight) _selectedHighlight.SetActive(_isSelected);

            // Truyền trực tiếp đối tượng definition
            if (_isSelected) _onSelect?.Invoke(definition);
            else _onDeselect?.Invoke(definition);
        }

        public void ForceSelectWithoutNotify()
        {
            _isSelected = true;
            if (_selectedHighlight) _selectedHighlight.SetActive(true);
        }

        public void ForceDeselectWithoutNotify()
        {
            _isSelected = false;
            if (_selectedHighlight) _selectedHighlight.SetActive(false);
        }
    }
}