using BeastLinkBattle.UI;
using UnityEngine;
using BeastLinkBattle.Gameplay.Level;
using BeastLinkBattle.UI.Views;

namespace BeastLinkBattle.UI.Presenters
{
    // LevelSelectionPresenter là một lớp chịu trách nhiệm kết nối giữa LevelSelectionView và các dịch vụ liên quan đến việc quản lý cấp độ.
    // Nó lắng nghe các sự kiện từ LevelSelectionView để xử lý khi người chơi
    // chọn một cấp độ, sau đó chuyển hướng người chơi đến PreBattleView với thông tin cấp độ đã chọn. 
    // Khi được dispose, nó sẽ hủy đăng ký tất cả các sự kiện để tránh rò rỉ bộ nhớ.
    public class LevelSelectionPresenter : MonoBehaviour
    {
        [SerializeField] private LevelSelectionView _view;
        [SerializeField] private PreBattlePresenter _preBattlePresenter;
        [SerializeField] private UIManager _uiManager;

        [Header("Model Data")]
        [SerializeField] private LevelData[] _allLevels;

        private bool _isInitialized = false;

        public void Initialize()
        {
            if (_isInitialized) return;

            // TODO: Load level data từ một nguồn dữ liệu thực tế (ví dụ: JSON, ScriptableObjects, hoặc một LevelManager) thay vì sử dụng dữ liệu cứng.
            int maxLevelUnlocked = PlayerPrefs.GetInt("MaxLevelUnlocked", 1);

            // Giả lập mỗi cấp độ có một số sao đã đạt được, lưu trữ trong PlayerPrefs với key "LevelStars_{levelNumber}"
            int[] starCounts = new int[_allLevels.Length];
            for (int i = 0; i < _allLevels.Length; i++)
            {
                starCounts[i] = PlayerPrefs.GetInt($"LevelStars_{_allLevels[i].levelId}", 0);
            }

            _view.PopulateLevelList(_allLevels, maxLevelUnlocked, starCounts);

            _view.OnLevelSelected += HandleLevelSelected;
            _isInitialized = true;
            Debug.Log("[LevelSelectionPresenter] Ðã load Tab B?n Ð?.");
        }

        private void HandleLevelSelected(LevelData selectedLevel)
        {
            _preBattlePresenter.SetSelectedLevel(selectedLevel);
            _uiManager.ShowPanel(PanelType.PreBattle, hideOthers: true);
        }

        private void OnDestroy()
        {
            if (_view != null) _view.OnLevelSelected -= HandleLevelSelected;
        }
    }
}