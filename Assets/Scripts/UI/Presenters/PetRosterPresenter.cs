using UnityEngine;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.UI.Views;
using BeastLinkBattle.Services;

namespace BeastLinkBattle.UI.Presenters
{
    public class PetRosterPresenter : MonoBehaviour
    {
        [SerializeField] private PetRosterView _view;

        private IInventoryService _inventoryService;
        private IDeckSessionService _deckSessionService;
        private bool _isInitialized = false;

        public void Setup(IInventoryService inventoryService, IDeckSessionService deckSessionService)
        {
            _inventoryService = inventoryService;
            _deckSessionService = deckSessionService;
        }

        public void Initialize()
        {
            if (_isInitialized) return;
            if (_inventoryService == null || _deckSessionService == null)
            {
                Debug.LogError("[PetRosterPresenter] Thiếu Service! Hãy gọi Setup() trước.");
                return;
            }

            _view.ShowLoadingState();

            // Lắng nghe khi kho đồ mở khóa thêm item
            _inventoryService.OnInventoryUpdated += RefreshRoster;

            // LẮNG NGHE STATE TỪ SESSION SERVICE
            _deckSessionService.OnDeckChanged += RefreshUIFromSession;

            RefreshRoster();

            _view.OnPetClicked += HandlePetClicked;
            _view.OnBeastSelected += HandleBeastToggled;
            _view.OnBeastDeselected += HandleBeastToggled; // Gọi chung hàm
            _view.OnEnergySelected += HandleEnergyToggled;
            _view.OnEnergyDeselected += HandleEnergyToggled; // Gọi chung hàm

            _isInitialized = true;
        }

        private void RefreshRoster()
        {
            List<BeastDefinition> unlockedBeasts = _inventoryService.GetUnlockedBeasts();
            List<BasePetDefinition> unlockedPets = _inventoryService.GetUnlockedPets();
            List<EnergyDefinition> unlockedEnergies = _inventoryService.GetUnlockedEnergies();

            _view.PopulateBeastRoster(unlockedBeasts);
            _view.PopulatePetRoster(unlockedPets);
            _view.PopulateEnergyRoster(unlockedEnergies);

            // Đồng bộ UI lần đầu tiên
            RefreshUIFromSession();
        }

        private void RefreshUIFromSession()
        {
            PlayerDeck deck = _deckSessionService.CurrentDeck;
            _view.SyncToggles(deck.LeaderPet, deck.SelectedBeasts, deck.SelectedEnergies);
        }

        public void ShowDefaultState()
        {
            if (_view != null) _view.ShowDefaultState();
        }

        // --- CHUYỂN GIAO THAO TÁC CHO SERVICE ---

        private void HandlePetClicked(BasePetDefinition pet)
        {
            _deckSessionService.SelectPet(pet);
        }

        private void HandleBeastToggled(BeastDefinition beast)
        {
            // ToggleBeast trả về false nếu bị đầy (limit). Lúc đó View lỡ click rồi nên ta ép render lại
            if (!_deckSessionService.ToggleBeast(beast))
            {
                RefreshUIFromSession();
            }
        }

        private void HandleEnergyToggled(EnergyDefinition energy)
        {
            if (!_deckSessionService.ToggleEnergy(energy))
            {
                RefreshUIFromSession();
            }
        }

        private void OnDestroy()
        {
            if (_deckSessionService != null)
                _deckSessionService.OnDeckChanged -= RefreshUIFromSession;

            if (_inventoryService != null)
                _inventoryService.OnInventoryUpdated -= RefreshRoster;

            if (_view != null)
            {
                _view.OnPetClicked -= HandlePetClicked;
                _view.OnBeastSelected -= HandleBeastToggled;
                _view.OnBeastDeselected -= HandleBeastToggled;
                _view.OnEnergySelected -= HandleEnergyToggled;
                _view.OnEnergyDeselected -= HandleEnergyToggled;
            }
        }
    }
}