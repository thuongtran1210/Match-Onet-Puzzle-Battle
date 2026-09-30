using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Nếu bạn dùng TextMeshPro
using BeastLinkBattle.UI.Data;

namespace BeastLinkBattle.UI.Views
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _buyButton;

        private ShopOffer _currentOffer;
        private Action<ShopOffer> _onBuyClickedCallback;

        private void Awake()
        {
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        public void Setup(ShopOffer offer, Action<ShopOffer> onBuyClicked)
        {
            _currentOffer = offer;
            _onBuyClickedCallback = onBuyClicked;
            _iconImage.sprite = offer.GetItemIcon();
            _nameText.text = offer.GetItemName();
            _priceText.text = offer.price.ToString();
        }

        private void OnBuyButtonClicked()
        {
            // Bắn callback ra ngoài khi nút được bấm
            _onBuyClickedCallback?.Invoke(_currentOffer);
        }
    }
}