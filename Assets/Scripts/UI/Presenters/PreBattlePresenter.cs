using BeastLinkBattle.UI;
using UnityEngine;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.Level;
using BeastLinkBattle.Core;
using BeastLinkBattle.UI.Views;
using BeastLinkBattle.Services;
using System.Linq;

namespace BeastLinkBattle.UI.Presenters
{
    public class PreBattlePresenter : MonoBehaviour
    {
        [SerializeField] private PreBattleView _view;
        [SerializeField] private GameBootstrapper _bootstrapper;
        [SerializeField] private UIManager _uiManager;

        [Header("Validation Requirements")]
        [SerializeField] private int _minBeastsRequired = 5;
        [SerializeField] private int _minEnergiesRequired = 5;

        private IInventoryService _inventoryService;
        private IDeckSessionService _deckSessionService; 

        private LevelData _selectedLevel;

  
        public void Setup(IInventoryService inventoryService, IDeckSessionService deckSessionService)
        {
            _inventoryService = inventoryService;
            _deckSessionService = deckSessionService;
        }

        public void Initialize()
        {
            if (_inventoryService == null || _deckSessionService == null)
            {
                Debug.LogError("[PreBattlePresenter] Cần gọi Setup() truyền đủ Service trước khi Initialize()!");
                return;
            }

            // Đổ dữ liệu kho vào View (List bên dưới)
            _view.PopulatePetList(_inventoryService.GetUnlockedPets());
            _view.PopulateBeastList(_inventoryService.GetUnlockedBeasts());
            _view.PopulateEnergyList(_inventoryService.GetUnlockedEnergies());

            // Lắng nghe sự thay đổi đội hình từ Service
            _deckSessionService.OnDeckChanged += RefreshUIFromSession;

            // Đăng ký sự kiện từ View
            _view.OnPetSelected += HandlePetSelected;
            _view.OnBeastSelected += HandleBeastSelected;
            _view.OnBeastDeselected += HandleBeastSelected;
            _view.OnEnergySelected += HandleEnergySelected;
            _view.OnEnergyDeselected += HandleEnergySelected;
            _view.OnStartClicked += HandleStartBattle;
            _view.OnBackClicked += HandleBack;

            // Render UI lần đầu dựa trên dữ liệu Session
            RefreshUIFromSession();
        }

        public void SetSelectedLevel(LevelData level)
        {
            _selectedLevel = level;
            _view.SetupLevelInfo(level);
            ValidateDeck();
        }

        // --- CẬP NHẬT UI TỪ SESSION ---
        private void RefreshUIFromSession()
        {
            PlayerDeck currentDeck = _deckSessionService.CurrentDeck;
            _view.SyncSavedDeckUI(currentDeck.LeaderPet, currentDeck.SelectedBeasts, currentDeck.SelectedEnergies);
            _view.UpdatePlayerSummary(currentDeck);

            ValidateDeck();
        }

        // --- CHUYỂN GIAO THAO TÁC CHO SERVICE ---
        private void HandlePetSelected(BasePetDefinition pet)
        {
            _deckSessionService.SelectPet(pet);
        }

        private void HandleBeastSelected(BeastDefinition beast)
        {
            // Hàm Toggle đã xử lý Limit, nếu Limit đầy nó sẽ return false và không làm gì.
            _deckSessionService.ToggleBeast(beast);
        }

        private void HandleEnergySelected(EnergyDefinition energy)
        {
            _deckSessionService.ToggleEnergy(energy);
        }

        // --- KIỂM SOÁT TÍNH HỢP LỆ VÀO TRẬN ---
        private void ValidateDeck()
        {
            PlayerDeck deck = _deckSessionService.CurrentDeck;

            // Kiểm tra từng điều kiện một
            bool hasPet = deck.LeaderPet != null;
            bool hasEnoughBeasts = deck.SelectedBeasts.Count >= _minBeastsRequired;
            bool hasEnoughEnergies = deck.SelectedEnergies.Count >= _minEnergiesRequired;
            bool hasLevel = _selectedLevel != null;

            // In log với màu sắc để dễ nhìn trong Console
            Debug.Log($"<color=cyan>[PreBattle Debug]</color> " +
                      $"| Pet: {hasPet} " +
                      $"| Beasts: {deck.SelectedBeasts.Count}/{_minBeastsRequired} " +
                      $"| Energies: {deck.SelectedEnergies.Count}/{_minEnergiesRequired} " +
                      $"| Level: {hasLevel}");

            bool isValid = hasPet && hasEnoughBeasts && hasEnoughEnergies && hasLevel;
            _view.SetStartButtonState(isValid);
        }

        private void HandleStartBattle()
        {
            PlayerDeck deck = _deckSessionService.CurrentDeck;
            if (deck.LeaderPet == null || _selectedLevel == null) return;
            if (deck.SelectedBeasts.Count < _minBeastsRequired || deck.SelectedEnergies.Count < _minEnergiesRequired) return;

            _uiManager.ShowPanel(PanelType.MainHUD, hideOthers: true);
            PlayerDeck battleDeck = new PlayerDeck(
                deck.LeaderPet,
                new List<BeastDefinition>(deck.SelectedBeasts),
                new List<EnergyDefinition>(deck.SelectedEnergies)
            );

            _bootstrapper.StartGame(_selectedLevel, battleDeck);
        }

        private void HandleBack()
        {
            _uiManager.ShowPanel(PanelType.MainMenu, hideOthers: true);
        }

        private void OnDestroy()
        {
            if (_deckSessionService != null)
                _deckSessionService.OnDeckChanged -= RefreshUIFromSession;

            if (_view != null)
            {
                _view.OnPetSelected -= HandlePetSelected;
                _view.OnBeastSelected -= HandleBeastSelected;
                _view.OnBeastDeselected -= HandleBeastSelected;
                _view.OnEnergySelected -= HandleEnergySelected;
                _view.OnEnergyDeselected -= HandleEnergySelected;
                _view.OnStartClicked -= HandleStartBattle;
                _view.OnBackClicked -= HandleBack;
            }
        }
    }
}