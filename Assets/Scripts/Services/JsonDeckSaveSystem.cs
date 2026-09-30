using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BeastLinkBattle.Gameplay.Data;
using System;

namespace BeastLinkBattle.Services
{
    /// <summary>
    /// Triển khai hệ thống lưu trữ đội hình sử dụng JSON.
    /// Ghi file vật lý tại thư mục an toàn của thiết bị (persistentDataPath).
    /// </summary>
    public class JsonDeckSaveSystem : IDeckSaveSystem
    {
        private readonly IInventoryService _inventoryService;
        private readonly string _saveDirectory;
        public event Action<PlayerDeck> OnDeckSaved;

        // Inject IInventoryService để lấy danh sách Pet/Beast gốc nhằm map ID ngược lại thành Object
        public JsonDeckSaveSystem(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;

            // Khởi tạo thư mục lưu trữ: VD: C:/Users/.../AppData/LocalLow/Company/Game/Saves/Decks
            _saveDirectory = Path.Combine(Application.persistentDataPath, "Saves", "Decks");

            if (!Directory.Exists(_saveDirectory))
            {
                Directory.CreateDirectory(_saveDirectory);
            }
        }

        private string GetFilePath(string slotId)
        {
            return Path.Combine(_saveDirectory, $"{slotId}.json");
        }

        public void SaveDeck(PlayerDeck deck, string slotId = "DefaultDeck")
        {
            if (deck == null) return;

            // 1. Chuyển đổi từ Model (PlayerDeck) sang DTO (DeckSaveData)
            DeckSaveData saveData = new DeckSaveData
            {
                LeaderPetId = deck.LeaderPet != null ? deck.LeaderPet.petId : string.Empty,
                SelectedBeastIds = deck.SelectedBeasts != null
                        ? deck.SelectedBeasts.Select(b => b.beastId).ToList()
                        : new List<string>(),
                // SỬA ĐỔI: Lưu danh sách energyId
                SelectedEnergyIds = deck.SelectedEnergies != null
                        ? deck.SelectedEnergies.Select(e => e.energyId).ToList()
                        : new List<string>()
            };

            // 2. Chuyển thành chuỗi JSON (true = pretty print cho dễ debug)
            string json = JsonUtility.ToJson(saveData, true);

            // 3. Ghi file
            File.WriteAllText(GetFilePath(slotId), json);
            OnDeckSaved?.Invoke(deck);
            //Debug.Log($"[JsonDeckSaveSystem] Đã lưu đội hình thành công tại: {GetFilePath(slotId)}");
        }

        public PlayerDeck LoadDeck(string slotId = "DefaultDeck")
        {
            string filePath = GetFilePath(slotId);

            if (!File.Exists(filePath))
            {
                Debug.Log($"[JsonDeckSaveSystem] Không tìm thấy file save cho slot: {slotId}");
                return null;
            }

            try
            {
                // 1. Đọc JSON từ file
                string json = File.ReadAllText(filePath);
                DeckSaveData saveData = JsonUtility.FromJson<DeckSaveData>(json);

                if (saveData == null) return null;

                // 2. Map ID ngược lại thành ScriptableObject từ kho Inventory

                // Lấy Leader Pet
                BasePetDefinition leader = _inventoryService.GetUnlockedPets()
                        .FirstOrDefault(p => p.petId == saveData.LeaderPetId);

                if (leader == null && !string.IsNullOrEmpty(saveData.LeaderPetId))
                {
                    Debug.LogWarning($"[JsonDeckSaveSystem] Pet mang ID '{saveData.LeaderPetId}' không tồn tại. Fallback về null để Presenter tự xử lý.");
                }

                // Lấy danh sách Beasts
                List<BeastDefinition> beasts = new List<BeastDefinition>();
                var unlockedBeasts = _inventoryService.GetUnlockedBeasts();

                foreach (string beastId in saveData.SelectedBeastIds)
                {
                    BeastDefinition foundBeast = unlockedBeasts.FirstOrDefault(b => b.beastId == beastId);
                    if (foundBeast != null)
                    {
                        beasts.Add(foundBeast);
                    }
                    else
                    {
                        Debug.LogWarning($"[JsonDeckSaveSystem] Beast mang ID '{beastId}' trong file save không tồn tại trong Inventory hiện tại. Bỏ qua.");
                    }
                }
                List<EnergyDefinition> energies = new List<EnergyDefinition>();
                var unlockedEnergies = _inventoryService.GetUnlockedEnergies();

                if (saveData.SelectedEnergyIds != null)
                {
                    foreach (string energyId in saveData.SelectedEnergyIds)
                    {
                        EnergyDefinition foundEnergy = unlockedEnergies.FirstOrDefault(e => e.energyId == energyId);
                        if (foundEnergy != null)
                        {
                            energies.Add(foundEnergy);
                        }
                        else
                        {
                            Debug.LogWarning($"[JsonDeckSaveSystem] Energy ID '{energyId}' không tồn tại trong kho.");
                        }
                    }
                }

                // 3. Khởi tạo lại PlayerDeck
                return new PlayerDeck(leader, beasts, energies);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[JsonDeckSaveSystem] Lỗi khi load file save {slotId}: {ex.Message}");
                return null;
            }
        }

        public bool HasSavedDeck(string slotId = "DefaultDeck")
        {
            return File.Exists(GetFilePath(slotId));
        }
    }
}