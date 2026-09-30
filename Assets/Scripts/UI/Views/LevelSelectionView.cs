using UnityEngine;
using System;
using BeastLinkBattle.Gameplay.Level;

namespace BeastLinkBattle.UI.Views
{

    // View tổng thể cho màn hình chọn level, quản lý việc hiển thị danh sách các node level và xử lý sự kiện khi người chơi chọn một level.
    public class LevelSelectionView : MonoBehaviour
    {
        [Header("UI Map Nodes")]
        [Tooltip("Các Prefab LevelNodeItemView ")]
        [SerializeField] private LevelNodeItemView[] _levelNodes;

        public event Action<LevelData> OnLevelSelected;

        // Phương thức này sẽ được gọi bởi LevelManager sau khi lấy được danh sách level có sẵn, level đã mở khóa và số sao đạt được cho mỗi level.
        public void PopulateLevelList(LevelData[] availableLevels, int maxLevelUnlocked, int[] starCounts)
        {
            for (int i = 0; i < _levelNodes.Length; i++)
            {
                if (i < availableLevels.Length)
                {
                    var levelData = availableLevels[i];
                    var nodeView = _levelNodes[i];
                    nodeView.gameObject.SetActive(true);

                    // Xác d?nh xem level này dã du?c m? chua
                    bool isUnlocked = levelData.levelId <= maxLevelUnlocked;
                    int stars = starCounts[i];

                    // G?i Setup trên node view
                    nodeView.Setup(levelData, isUnlocked, stars, (selectedLevel) =>
                    {
                        OnLevelSelected?.Invoke(selectedLevel);
                    });
                }
                else
                {
                    _levelNodes[i].gameObject.SetActive(false);
                }
            }
        }
    }
}