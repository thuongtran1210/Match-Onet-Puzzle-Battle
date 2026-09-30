using BeastLinkBattle.Grid.Data;
// Scripts/Grid/View/EnergyUIConfig.cs
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using BeastLinkBattle.Gameplay.Data;

namespace BeastLinkBattle.UI.Views
{
    // ScriptableObject này sẽ chứa cấu hình hiển thị cho các loại năng lượng khác nhau trong game,
    // bao gồm màu sắc của thanh năng lượng và biểu tượng tương ứng, 
    // giúp cho việc hiển thị năng lượng trên UI trở nên linh hoạt và dễ dàng tùy chỉnh thông qua Unity Editor.   
    public class EnergyUIConfig : ScriptableObject
    {
        public List<EnergyVisualData> visuals;

        [Serializable]
        public class EnergyVisualData
        {
            // Thay đổi từ EnergyType sang EnergyDefinition
            public EnergyDefinition definition;
            public Color barColor = Color.white;
            public Sprite icon;
        }

        // Cập nhật hàm lấy dữ liệu visual theo định nghĩa năng lượng
        public EnergyVisualData GetVisual(EnergyDefinition def) =>
            visuals.FirstOrDefault(x => x.definition == def);
    }
}