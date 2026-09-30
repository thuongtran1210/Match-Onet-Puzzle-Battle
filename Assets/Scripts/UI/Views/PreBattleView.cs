using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.Level;
using TMPro;
using System.Linq;

namespace BeastLinkBattle.UI.Views
{
    public class PreBattleView : MonoBehaviour
    {
        [Header("Roster Sub-View")]
        [SerializeField] private PetRosterView _rosterView;

        [Header("Buttons")]
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _backBtn;

        [Header("Level Detail Area")]
        [SerializeField] private TextMeshProUGUI _levelTitleDetailText;
        [SerializeField] private TextMeshProUGUI _waveCountText;
        [SerializeField] private TextMeshProUGUI _gridShapeText;
        [SerializeField] private TextMeshProUGUI _respawnPolicyText;
        [SerializeField] private TextMeshProUGUI _enemyElementsText;
        [SerializeField] private TextMeshProUGUI _enemyTotalPowerText;

        [Header("Enemy Lineup")]
        [SerializeField] private Transform _enemyLineupContainer;
        [SerializeField] private GameObject _enemyIconPrefab;

        // THÊM MỚI: Khu vực hiển thị thông tin tổng quan của Player
        [Header("Player Deck Summary")]
        [SerializeField] private TextMeshProUGUI _playerTotalPowerText;
        [SerializeField] private TextMeshProUGUI _equippedEnergiesText;

        // --- EVENTS ---
        public event Action<BasePetDefinition> OnPetSelected;
        public event Action OnStartClicked;
        public event Action OnBackClicked;

        public event Action<BeastDefinition> OnBeastSelected;
        public event Action<BeastDefinition> OnBeastDeselected;

        public event Action<EnergyDefinition> OnEnergySelected;
        public event Action<EnergyDefinition> OnEnergyDeselected;

        private void Awake()
        {
            if (_startBtn) _startBtn.onClick.AddListener(() => OnStartClicked?.Invoke());
            if (_backBtn) _backBtn.onClick.AddListener(() => OnBackClicked?.Invoke());

            if (_rosterView != null)
            {
                _rosterView.OnPetClicked += (p) => OnPetSelected?.Invoke(p);
                _rosterView.OnBeastSelected += (b) => OnBeastSelected?.Invoke(b);
                _rosterView.OnBeastDeselected += (b) => OnBeastDeselected?.Invoke(b);
                _rosterView.OnEnergySelected += (e) => OnEnergySelected?.Invoke(e);
                _rosterView.OnEnergyDeselected += (e) => OnEnergyDeselected?.Invoke(e);
            }
        }

        public void SetupLevelInfo(LevelData level)
        {
            if (level == null) return;

            // Chi tiết Level cơ bản
            if (_levelTitleDetailText) _levelTitleDetailText.text = level.levelName;
            if (_waveCountText) _waveCountText.text = $"Tổng số Wave: {level.waves?.Count ?? 0}";
            if (_gridShapeText) _gridShapeText.text = $"Bản đồ: {(level.shape != null ? level.shape.name : "Mặc định")}";
            if (_respawnPolicyText) _respawnPolicyText.text = $"Luật rơi: {(level.respawnPolicy != null ? level.respawnPolicy.name : "Cơ bản")}";

            CalculateAndDisplayEnemyStats(level);

            // Lineup kẻ địch
            if (_enemyLineupContainer != null && _enemyIconPrefab != null)
            {
                foreach (Transform child in _enemyLineupContainer) Destroy(child.gameObject);

                if (level.waves != null && level.waves.Count > 0)
                {
                    var distinctEnemies = level.waves[0].enemies
                        .Select(e => e.definition)
                        .Where(d => d != null)
                        .Distinct();

                    foreach (var enemyDef in distinctEnemies)
                    {
                        GameObject iconObj = Instantiate(_enemyIconPrefab, _enemyLineupContainer);
                        Image img = iconObj.GetComponentInChildren<Image>();
                        if (img != null) img.sprite = enemyDef.uiIcon;
                    }
                }
            }
        }

        private void CalculateAndDisplayEnemyStats(LevelData level)
        {
            if (level.waves == null) return;

            HashSet<ElementType> uniqueElements = new HashSet<ElementType>();
            int totalPower = 0;

            foreach (var wave in level.waves)
            {
                if (wave.enemies == null) continue;

                foreach (var spawn in wave.enemies)
                {
                    if (spawn.definition == null) continue;
                    uniqueElements.Add(spawn.definition.element);

                    int basePower = spawn.definition.baseHp + (spawn.definition.baseDamage * 3);
                    int spawnPower = basePower * spawn.starLevel * spawn.count;

                    if (spawn.isMiniBoss) spawnPower *= 2;
                    totalPower += spawnPower;
                }
            }

            if (_enemyElementsText)
            {
                string elementsString = string.Join(", ", uniqueElements);
                _enemyElementsText.text = $"Hệ kẻ địch: {elementsString}";
            }

            if (_enemyTotalPowerText)
            {
                _enemyTotalPowerText.text = $"Tổng lực chiến: {totalPower:N0}";
            }
        }

        // THÊM MỚI: Tính toán và hiển thị thông tin Đội hình quân ta
        public void UpdatePlayerSummary(PlayerDeck deck)
        {
            if (deck == null) return;

            // 1. Tính tổng lực chiến quân ta
            int playerTotalPower = 0;
            if (deck.SelectedBeasts != null)
            {
                foreach (var beast in deck.SelectedBeasts)
                {
                    if (beast != null)
                    {
                        // Sử dụng cùng công thức với quái: BaseHp + (BaseDamage * 3)
                        playerTotalPower += beast.baseHp + (beast.baseDamage * 3);
                    }
                }
            }

            if (_playerTotalPowerText)
            {
                _playerTotalPowerText.text = $"Lực chiến quân ta: {playerTotalPower:N0}";
            }

            // 2. Hiển thị Ngọc đang trang bị
            if (_equippedEnergiesText)
            {
                if (deck.SelectedEnergies != null && deck.SelectedEnergies.Count > 0)
                {
                    // Lấy tên các loại ngọc đang trang bị để hiển thị
                    var energyNames = deck.SelectedEnergies.Where(e => e != null).Select(e => e.name); // Hoặc e.displayName nếu bạn có
                    _equippedEnergiesText.text = $"Ngọc trang bị: {deck.SelectedEnergies.Count} ({string.Join(", ", energyNames)})";
                }
                else
                {
                    _equippedEnergiesText.text = "Ngọc trang bị: 0";
                }
            }
        }

        public void PopulatePetList(IEnumerable<BasePetDefinition> availablePets) => _rosterView?.PopulatePetRoster(availablePets);
        public void PopulateBeastList(IEnumerable<BeastDefinition> availableBeasts) => _rosterView?.PopulateBeastRoster(availableBeasts);
        public void PopulateEnergyList(IEnumerable<EnergyDefinition> availableEnergies) => _rosterView?.PopulateEnergyRoster(availableEnergies);

        public void SetStartButtonState(bool isValid)
        {
            if (_startBtn != null) _startBtn.interactable = isValid;
        }

        public void SyncSavedDeckUI(BasePetDefinition pet, List<BeastDefinition> savedBeasts, List<EnergyDefinition> savedEnergies)
        {
            if (_rosterView != null)
            {
                _rosterView.SyncToggles(pet, savedBeasts, savedEnergies);
            }
        }
    }
}