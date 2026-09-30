using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BeastLinkBattle.UI.Data;

namespace BeastLinkBattle.UI.Views
{
    public class ShopView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform _packContainer;
        [SerializeField] private ShopItemView _shopItemPrefab;

        [Header("Confirm Popup")]
        [SerializeField] private GameObject _confirmPopup;
        [SerializeField] private TextMeshProUGUI _confirmText;
        [SerializeField] private Button _btnConfirmYes;
        [SerializeField] private Button _btnConfirmNo;

        [Header("Success Popup")]
        [SerializeField] private GameObject _successPopup;
        [SerializeField] private TextMeshProUGUI _successText;
        [SerializeField] private Image _successIcon;
        [SerializeField] private ParticleSystem _successParticles;
        [SerializeField] private AudioSource _successAudio;
        [SerializeField] private Button _btnSuccessClose;

        [Header("Error Popup")]
        [SerializeField] private GameObject _errorPopup;
        [SerializeField] private Button _btnErrorClose;

        public event Action<ShopOffer> OnBuyOfferRequested;

        private ShopOffer _pendingOffer; 

        private void Awake()
        {
            // Đăng ký sự kiện cho các nút trong Popups
            _btnConfirmYes.onClick.AddListener(OnConfirmPurchase);
            _btnConfirmNo.onClick.AddListener(CloseAllPopups);
            _btnSuccessClose.onClick.AddListener(CloseAllPopups);
            _btnErrorClose.onClick.AddListener(CloseAllPopups);

            CloseAllPopups(); // Đảm bảo ẩn hết khi khởi tạo
        }

        private void ClearContainer()
        {
            foreach (Transform child in _packContainer)
            {
                Destroy(child.gameObject);
            }
        }

        public void DisplayOffers(List<ShopOffer> offers)
        {
            ClearContainer();

            foreach (var offer in offers)
            {
                ShopItemView itemObj = Instantiate(_shopItemPrefab, _packContainer);
                itemObj.Setup(offer, HandleItemClicked);
            }
        }

        // Thay vì mua ngay, hiện Popup xác nhận
        private void HandleItemClicked(ShopOffer clickedOffer)
        {
            _pendingOffer = clickedOffer;

            _confirmText.text = $"Bạn có chắc chắn muốn mua\n<color=#FFD700>{_pendingOffer.GetItemName()}</color>\nvới giá {_pendingOffer.price} Vàng không?";

            _confirmPopup.SetActive(true);
        }

        // Khi người chơi bấm "Yes"
        private void OnConfirmPurchase()
        {
            _confirmPopup.SetActive(false);
            OnBuyOfferRequested?.Invoke(_pendingOffer);
        }

        // Cập nhật hàm này để nhận tham số ShopOffer thay vì string
        public void ShowPurchaseSuccess(ShopOffer purchasedOffer)
        {
            _successPopup.SetActive(true);

            _successText.text = $"Chúc mừng!\nBạn đã nhận được {purchasedOffer.GetItemName()}";
            _successIcon.sprite = purchasedOffer.GetItemIcon();

            // Chạy hiệu ứng
            if (_successParticles != null) _successParticles.Play();
            if (_successAudio != null) _successAudio.Play();
        }

        public void ShowNotEnoughMoneyError()
        {
            _errorPopup.SetActive(true);
        }

        private void CloseAllPopups()
        {
            _confirmPopup.SetActive(false);
            _successPopup.SetActive(false);
            _errorPopup.SetActive(false);
            _pendingOffer = null;
        }

        private void OnDestroy()
        {
            _btnConfirmYes.onClick.RemoveAllListeners();
            _btnConfirmNo.onClick.RemoveAllListeners();
            _btnSuccessClose.onClick.RemoveAllListeners();
            _btnErrorClose.onClick.RemoveAllListeners();
        }
    }
}