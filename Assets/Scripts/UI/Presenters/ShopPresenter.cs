using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BeastLinkBattle.UI.Views;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Grid.Data; // Chứa EnergyType
using BeastLinkBattle.Services;
using BeastLinkBattle.UI.Data;

namespace BeastLinkBattle.UI.Presenters
{
    public class ShopPresenter : MonoBehaviour
    {
        [SerializeField] private ShopView _view;

        [Header("Shop Database")]
        [SerializeField] private List<ShopOffer> _allAvailableOffers = new List<ShopOffer>();

        private IInventoryService _inventoryService;
        private ICurrencyService _currencyService;
        private bool _isInitialized = false;

        public void Setup(IInventoryService inventoryService, ICurrencyService currencyService)
        {
            _inventoryService = inventoryService;
            _currencyService = currencyService;
        }

        public void Initialize()
        {
            if (_isInitialized) return;

            _view.OnBuyOfferRequested += HandleBuyOffer;
            RefreshShopDisplay();

            _isInitialized = true;
        }

        private void RefreshShopDisplay()
        {
            if (_view == null || _inventoryService == null || _currencyService == null) return;

            if (_allAvailableOffers == null) _allAvailableOffers = new List<ShopOffer>();

            // Lấy danh sách đã sở hữu
            var unlockedBeasts = _inventoryService.GetUnlockedBeasts() ?? new List<BeastDefinition>();
            var unlockedPets = _inventoryService.GetUnlockedPets() ?? new List<BasePetDefinition>();
            var unlockedEnergies = _inventoryService.GetUnlockedEnergies() ?? new List<EnergyDefinition>();

            // Lọc ra các offer mà người chơi CHƯA sở hữu
            var offersToShow = _allAvailableOffers.Where(offer =>
            {
                if (offer == null) return false;

                switch (offer.offerType)
                {
                    case OfferType.Beast:
                        return offer.beastReward != null && !unlockedBeasts.Contains(offer.beastReward);
                    case OfferType.Pet:
                        return offer.petReward != null && !unlockedPets.Contains(offer.petReward);
                    case OfferType.Energy:
                        return offer.energyReward != null && !unlockedEnergies.Contains(offer.energyReward);
                    default:
                        return false;
                }
            }).ToList();

            _view.DisplayOffers(offersToShow);
        }

        private void HandleBuyOffer(ShopOffer offer)
        {
            bool hasEnoughMoney = _currencyService.SpendCurrency(CurrencyType.Gold, offer.price);

            if (hasEnoughMoney && _inventoryService != null)
            {
                // Phân rẽ logic Mở khoá tuỳ theo loại Offer
                switch (offer.offerType)
                {
                    case OfferType.Beast:
                        _inventoryService.UnlockBeast(offer.beastReward);
                        break;
                    case OfferType.Pet:
                        _inventoryService.UnlockPet(offer.petReward);
                        break;
                    case OfferType.Energy:
                        _inventoryService.UnlockEnergy(offer.energyReward);
                        break;
                }

                Debug.Log($"Đã mua và mở khóa thành công: {offer.GetItemName()}");

                _view.ShowPurchaseSuccess(offer);
                RefreshShopDisplay();
            }
            else
            {
                Debug.LogWarning("Không đủ tiền để mua!");
                _view.ShowNotEnoughMoneyError();
            }
        }

        private void OnDestroy()
        {
            if (_view != null)
            {
                _view.OnBuyOfferRequested -= HandleBuyOffer;
            }
        }
    }
}