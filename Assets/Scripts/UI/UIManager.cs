using UnityEngine;
using System.Collections.Generic;

namespace BeastLinkBattle.UI
{
    public enum PanelType
    {
        Header,         // Chứa CurrencyView và có thể thêm các thông tin khác như tên người chơi, level hiện tại, v.v.
        MainMenu,       // 
        LevelSelection, // Chứa danh sách level và nút bắt đầu trận đấu.
        PreBattle,      // 
        SelectedDeck,   // Chứa thông tin đội hình đã chọn trước khi vào trận đấu, có thể cho phép chỉnh sửa nhanh.
        MainHUD,        // Chứa HealthBar, WaveInfo, SkillButtons, v.v. 
        BattleQueueHUD, // Chứa danh sách thú cưng đang chờ ra trận.
        Pause,
        GameOver,       // Chứa thông tin kết quả trận đấu và nút quay lại menu chính.
        WaveTransition, // Chứa thông tin về wave đã hoàn thành và chuẩn bị cho wave tiếp theo.
        Loading         // Chứa màn hình tải khi bắt đầu trận đấu hoặc chuyển đổi giữa các màn hình.
    }

    public class UIManager : MonoBehaviour, IUIFlowService
    {
        public static UIManager Instance { get; private set; }

        [System.Serializable]
        public class PanelConfig
        {
            public PanelType type;
            public CanvasGroup canvasGroup;
            public GameObject panelObject; 
        }

        public List<PanelConfig> panels;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ShowPanel(PanelType type, bool hideOthers = false)
        {
            foreach (var panel in panels)
            {
                if (panel.type == type)
                {
                    SetPanelState(panel, true);
                }
                else if (hideOthers)
                {
                    if (panel.type == PanelType.Header) continue;

                    // Nếu đang mở Popup (Pause/GameOver), chỉ khóa tương tác MainHUD chứ không làm mờ/tắt hẳn
                    if (IsPopup(type) && panel.type == PanelType.MainHUD)
                    {
                        if (panel.canvasGroup != null) panel.canvasGroup.interactable = false;
                    }
                    else
                    {
                        SetPanelState(panel, false);
                    }
                }
            }
        }

        public void HidePanel(PanelType type)
        {
            foreach (var panel in panels)
            {
                if (panel.type == type)
                {
                    SetPanelState(panel, false);

                    // N?u t?t Popup, m? l?i tuong tác cho MainHUD
                    if (IsPopup(type))
                    {
                        var mainHud = panels.Find(p => p.type == PanelType.MainHUD);
                        if (mainHud != null && mainHud.canvasGroup != null) mainHud.canvasGroup.interactable = true;
                    }
                }
            }
        }

        private bool IsPopup(PanelType type)
        {
            return type == PanelType.Pause || type == PanelType.GameOver || type == PanelType.WaveTransition;
        }

        private void SetPanelState(PanelConfig panel, bool isActive)
        {
            if (panel.canvasGroup != null)
            {
                panel.canvasGroup.alpha = isActive ? 1f : 0f;
                panel.canvasGroup.interactable = isActive;
                panel.canvasGroup.blocksRaycasts = isActive;
            }
            if (panel.panelObject != null)
            {
                panel.panelObject.SetActive(isActive);
            }
        }


    }
}
