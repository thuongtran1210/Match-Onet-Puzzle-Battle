using UnityEngine;
using UnityEngine.UI;
using System;

namespace BeastLinkBattle.UI.Views
{
    /// <summary>
    /// Class dùng chung để hiển thị Pet, Beast, hoặc Energy trong SelectedDeckView.
    /// </summary>
    public class DeckItemView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _bgImage;


        private Action _onClickAction;



        public void Setup(Sprite iconSprite, Color bgColor, Action onClick = null)
        {
            Debug.Log($"Đang setup DeckItemView. _iconImage null? {_iconImage == null}, iconSprite null? {iconSprite == null}");
            _onClickAction = onClick;

            if (_iconImage != null && iconSprite != null)
            {
                _iconImage.sprite = iconSprite;
            }

            if (_bgImage != null)
            {
                _bgImage.color = bgColor;
            }

        }

        private void OnButtonClicked()
        {
            _onClickAction?.Invoke();
        }
    }
}