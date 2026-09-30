using System;
using System.Collections.Generic;
using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Gameplay.Data; // Thêm namespace chứa EnergyDefinition
using UnityEngine;

namespace BeastLinkBattle.Gameplay
{
    public class EnergyManager : IEnergyService
    {
        // Thay đổi sang EnergyDefinition
        public event Action<EnergyDefinition, int, int> OnEnergyChanged;
        public event Action<EnergyDefinition> OnEnergyFull;

        private EnergyConfig _config;
        private Dictionary<EnergyDefinition, int> _currentEnergies = new Dictionary<EnergyDefinition, int>();

        public EnergyManager(EnergyConfig config)
        {
            _config = config;
            // Đã xóa vòng lặp Enum.GetValues.
            // Dictionary sẽ tự động khởi tạo giá trị khi có viên Energy đầu tiên bị phá.
        }

        public void ProcessMatchedContent(ITileContent content)
        {
            if (content is EnergyContent energy)
            {
                EnergyDefinition def = energy.Definition; // Sử dụng Definition

                // Khởi tạo nếu loại năng lượng này chưa có trong Dictionary
                if (!_currentEnergies.ContainsKey(def))
                {
                    _currentEnergies[def] = 0;
                }

                int maxEnergy = _config.GetMaxEnergy(def);

                if (_currentEnergies[def] >= maxEnergy) return;

                _currentEnergies[def] += _config.energyPerMatch;

                if (_currentEnergies[def] >= maxEnergy)
                {
                    _currentEnergies[def] = maxEnergy;
                    // Lấy displayName từ ScriptableObject để log đẹp hơn
                    Debug.Log($"[EnergyManager] Năng lượng {def.displayName} ĐÃ ĐẦY! Sẵn sàng dùng Skill.");
                    OnEnergyFull?.Invoke(def);
                }

                OnEnergyChanged?.Invoke(def, _currentEnergies[def], maxEnergy);
            }
        }

        public bool TryConsumeEnergyForSkill(EnergyDefinition def) // Sử dụng Definition
        {
            if (!_currentEnergies.ContainsKey(def)) return false;

            int maxEnergy = _config.GetMaxEnergy(def);

            if (_currentEnergies[def] >= maxEnergy)
            {
                _currentEnergies[def] = 0;

                Debug.Log($"[EnergyManager] Đã tiêu hao năng lượng {def.displayName} để cast Skill.");
                OnEnergyChanged?.Invoke(def, _currentEnergies[def], maxEnergy);
                return true;
            }

            Debug.LogWarning($"[EnergyManager] Không đủ năng lượng {def.displayName} để cast Skill!");
            return false;
        }
    }
}