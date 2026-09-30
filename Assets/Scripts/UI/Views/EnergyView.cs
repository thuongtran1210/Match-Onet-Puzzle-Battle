using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.UI.Views;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnergyView : MonoBehaviour
{
    [SerializeField] private EnergyBarView _energyBarPrefab; // Prefab của nút/thanh năng lượng
    [SerializeField] private Transform _container; // Layout Group chứa các nút

    private Dictionary<EnergyDefinition, EnergyBarView> _bars = new Dictionary<EnergyDefinition, EnergyBarView>();
    public event Action<EnergyDefinition> OnSkillButtonClicked;

    // Hàm khởi tạo danh sách nút
    public void Initialize(List<EnergyDefinition> definitions)
    {
        foreach (var def in definitions)
        {
            var bar = Instantiate(_energyBarPrefab, _container);
            bar.Setup(def); // Gắn data (icon, color) vào nút

            // Lắng nghe sự kiện click từ EnergyBarView
            bar.OnSkillButtonClicked += (clickedDef) => OnSkillButtonClicked?.Invoke(clickedDef);

            _bars[def] = bar;
        }
    }

    // Cập nhật thanh năng lượng
    public void UpdateEnergyBar(EnergyDefinition def, int current, int max)
    {
        if (_bars.TryGetValue(def, out var bar))
        {
            bar.UpdateBar(current, max);
        }
    }

    // Bổ sung hàm này để xử lý sự kiện OnEnergyFull từ Presenter
    public void ShowFullState(EnergyDefinition def)
    {
        if (_bars.TryGetValue(def, out var bar))
        {
            bar.ShowFullState();
        }
    }
}