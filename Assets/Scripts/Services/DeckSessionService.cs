using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nguồn dữ liệu duy nhất (Single Source of Truth) quản lý đội hình trong Runtime.
/// Tự động đồng bộ xuống file JSON mỗi khi có thay đổi.
/// </summary>
public class DeckSessionService : IDeckSessionService
{
    private readonly IDeckSaveSystem _saveSystem;
    private readonly IInventoryService _inventoryService;

    // Cấu hình giới hạn tập trung (Bạn có thể đổi giá trị mặc định ở đây)
    public int MaxBeasts { get; set; } = 5;
    public int MaxEnergies { get; set; } = 5;

    // Trạng thái (State) thực tế lưu trên RAM
    private BasePetDefinition _currentPet;
    private List<BeastDefinition> _currentBeasts = new List<BeastDefinition>();
    private List<EnergyDefinition> _currentEnergies = new List<EnergyDefinition>();

    // Tạo ra một object PlayerDeck mỗi khi có nơi gọi để đảm bảo tính đóng gói an toàn
    public PlayerDeck CurrentDeck => new PlayerDeck(
        _currentPet,
        new List<BeastDefinition>(_currentBeasts),
        new List<EnergyDefinition>(_currentEnergies)
    );

    public event Action OnDeckChanged;

    public DeckSessionService(IDeckSaveSystem saveSystem, IInventoryService inventoryService)
    {
        _saveSystem = saveSystem;
        _inventoryService = inventoryService;
    }

    public void Initialize()
    {
        // 1. Load đội hình đã lưu trước đó
        PlayerDeck savedDeck = _saveSystem.LoadDeck("DefaultDeck");

        if (savedDeck != null)
        {
            //Debug.Log($"[DeckSession] Load thành công. Pet: {(savedDeck.LeaderPet != null ? savedDeck.LeaderPet.name : "NULL")}");
            _currentPet = savedDeck.LeaderPet;
            if (savedDeck.SelectedBeasts != null) _currentBeasts = new List<BeastDefinition>(savedDeck.SelectedBeasts);
            if (savedDeck.SelectedEnergies != null) _currentEnergies = new List<EnergyDefinition>(savedDeck.SelectedEnergies);
        }

        // 2. FALLBACK: Tự động gán Pet mặc định nếu file save bị lỗi hoặc người chơi mới tinh
        if (_currentPet == null)
        {
            List<BasePetDefinition> unlockedPets = _inventoryService.GetUnlockedPets();
            if (unlockedPets != null && unlockedPets.Count > 0)
            {
                _currentPet = unlockedPets[0];
                //Debug.Log("[DeckSession] Khởi tạo lần đầu: Tự động gán Pet mặc định.");
            }
        }

        // 3. Chuẩn hóa: Cắt đuôi nếu file save cũ lưu nhiều hơn mức Limit hiện tại
        if (_currentBeasts.Count > MaxBeasts) _currentBeasts = _currentBeasts.GetRange(0, MaxBeasts);
        if (_currentEnergies.Count > MaxEnergies) _currentEnergies = _currentEnergies.GetRange(0, MaxEnergies);

        // Lưu cấu trúc chuẩn hóa xuống file ngay lập tức
        SaveAndNotify();
        //Debug.Log("[DeckSession] Đã khởi tạo Session Đội hình thành công!");
    }

    public void SelectPet(BasePetDefinition pet)
    {
        if (pet == null || _currentPet == pet) return;

        _currentPet = pet;
        SaveAndNotify();
    }

    public bool ToggleBeast(BeastDefinition beast)
    {
        if (_currentBeasts.Contains(beast))
        {
            _currentBeasts.Remove(beast);
            SaveAndNotify();
            return true;
        }
        else
        {
            if (_currentBeasts.Count >= MaxBeasts)
            {
                Debug.LogWarning($"[DeckSession] Không thể thêm Beast. Đã đạt giới hạn tối đa {MaxBeasts}.");
                return false;
            }
            _currentBeasts.Add(beast);
            SaveAndNotify();
            return true;
        }
    }

    public bool ToggleEnergy(EnergyDefinition energy)
    {
        if (_currentEnergies.Contains(energy))
        {
            _currentEnergies.Remove(energy);
            SaveAndNotify();
            return true;
        }
        else
        {
            if (_currentEnergies.Count >= MaxEnergies)
            {
                Debug.LogWarning($"[DeckSession] Không thể thêm Energy. Đã đạt giới hạn tối đa {MaxEnergies}.");
                return false;
            }
            _currentEnergies.Add(energy);
            SaveAndNotify();
            return true;
        }
    }

    private void SaveAndNotify()
    {
        // Log ra trạng thái thực tế đang nằm trên RAM
        Debug.Log($"<color=cyan>[DeckSession]</color> Đang lưu đội hình... " +
                  $"Pet: {(_currentPet != null ? _currentPet.name : "None")} | " +
                  $"Tổng Beast đang chọn: {_currentBeasts.Count} | " +
                  $"Tổng Energy đang chọn: {_currentEnergies.Count}");

        PlayerDeck newDeck = CurrentDeck;
        _saveSystem.SaveDeck(newDeck, "DefaultDeck"); // Lưu xuống file JSON
        OnDeckChanged?.Invoke();                      // Bắn sự kiện cho các Presenter vẽ lại UI
    }
}
