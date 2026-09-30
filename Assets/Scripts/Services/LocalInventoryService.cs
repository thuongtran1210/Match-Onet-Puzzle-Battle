using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Grid.Data;
using System;

namespace BeastLinkBattle.Services
{
    // Lưu trữ thông tin về các Pet, Beast, Energy mà người chơi đã mở khóa.
    // Dữ liệu được lưu trữ cục bộ trên thiết bị bằng JSON, sử dụng petId và beastId để map ngược lại thành Object khi load.
    public class LocalInventoryService : MonoBehaviour, IInventoryService
    {
        [Header("Master Database (Kéo TẤT CẢ item của game vào đây)")]
        [SerializeField] private List<BasePetDefinition> _allGamePets;
        [SerializeField] private List<BeastDefinition> _allGameBeasts;
        [SerializeField] private List<EnergyDefinition> _allGameEnergies;

        [Header("Starter Pack (Sẽ tặng khi tạo acc mới)")]
        [SerializeField] private BasePetDefinition _starterPet;
        [SerializeField] private List<BeastDefinition> _starterBeasts;
        [SerializeField] private List<EnergyDefinition> _starterEnergies;

        // Dữ liệu bộ nhớ tạm (Cache)
        private List<BasePetDefinition> _unlockedPets = new List<BasePetDefinition>();
        private List<BeastDefinition> _unlockedBeasts = new List<BeastDefinition>();
        private List<EnergyDefinition> _unlockedEnergies = new List<EnergyDefinition>();

        public event Action OnInventoryUpdated;
        private string _savePath;

        public void Initialize()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "Saves", "PlayerInventory.json");
            LoadInventory();
        }

        // --- CÁC HÀM GET ---
        public List<BasePetDefinition> GetUnlockedPets() => _unlockedPets;
        public List<BeastDefinition> GetUnlockedBeasts() => _unlockedBeasts;
        public List<EnergyDefinition> GetUnlockedEnergies() => _unlockedEnergies;

        // --- CÁC HÀM UNLOCK ---
        public void UnlockEnergy(EnergyDefinition energy)
        {
            if (energy == null) return;

            if (!_unlockedEnergies.Contains(energy))
            {
                _unlockedEnergies.Add(energy);
                SaveInventory();
                OnInventoryUpdated?.Invoke();
            }
        }

        public void UnlockBeast(BeastDefinition beast)
        {
            if (beast == null) return;

            if (!_unlockedBeasts.Contains(beast))
            {
                _unlockedBeasts.Add(beast);
                SaveInventory();
                OnInventoryUpdated?.Invoke();
            }
        }

        public void UnlockPet(BasePetDefinition pet)
        {
            if (pet == null) return;

            if (!_unlockedPets.Contains(pet))
            {
                _unlockedPets.Add(pet);
                SaveInventory();
                OnInventoryUpdated?.Invoke(); // Thông báo cho UI cập nhật
            }
        }

        // --- LOGIC SAVE / LOAD ---
        public void SaveInventory()
        {
            InventorySaveData data = new InventorySaveData
            {
                UnlockedPetIds = _unlockedPets.Select(p => p.petId).ToList(),
                UnlockedBeastIds = _unlockedBeasts.Select(b => b.beastId).ToList(),
                // Lưu ID thay vì Enum để đảm bảo tính nhất quán
                UnlockedEnergyIds = _unlockedEnergies.Select(e => e.energyId).ToList()
            };

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
        }

        private void LoadInventory()
        {
            if (File.Exists(_savePath))
            {

                string json = File.ReadAllText(_savePath);
                InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

                _unlockedPets = _allGamePets.Where(p => data.UnlockedPetIds.Contains(p.petId)).ToList();
                _unlockedBeasts = _allGameBeasts.Where(b => data.UnlockedBeastIds.Contains(b.beastId)).ToList();
                // Map lại từ ID sang ScriptableObject
                _unlockedEnergies = _allGameEnergies.Where(e => data.UnlockedEnergyIds.Contains(e.energyId)).ToList();

                OnInventoryUpdated?.Invoke();
            }
            else
            {
                // Người chơi mới -> Cấp Starter Pack và Lưu lần đầu
                _unlockedPets.Add(_starterPet);
                _unlockedBeasts.AddRange(_starterBeasts);
                _unlockedEnergies.AddRange(_starterEnergies);

                string dir = Path.GetDirectoryName(_savePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                SaveInventory();
            }
        }
    }
}