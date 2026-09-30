using UnityEngine;
using UnityEngine.UI;
using System;
using BeastLinkBattle.Gameplay.Data;

namespace BeastLinkBattle.UI.Views
{
    public class PetButtonView : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("Component Button")]
        [SerializeField] private Button _button;

        [Tooltip("Kéo object Image con dùng để hiển thị icon thú cưng vào đây")]
        [SerializeField] private Image _iconImage;

        private BasePetDefinition _myPet;
        public BasePetDefinition MyDef => _myPet;
        private Action<BasePetDefinition> _onClickCallback;

        public void Setup(BasePetDefinition pet, Action<BasePetDefinition> onClick)
        {
            _myPet = pet;
            _onClickCallback = onClick;

            if (_iconImage != null && pet.uiIcon != null)
            {
                _iconImage.sprite = pet.uiIcon;
            }

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(HandleClick);
        }
        public void ForceSelectWithoutNotify()
        {
            // Bật khung viền highlight hoặc hiệu ứng UI cho Pet đang được chọn
            // Ví dụ: if (_selectedHighlight) _selectedHighlight.SetActive(true);
        }

        private void HandleClick()
        {
            _onClickCallback?.Invoke(_myPet);
        }
    }
}