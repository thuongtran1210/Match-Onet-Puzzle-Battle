using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening; // Vẫn giữ lại DOTween để làm hiệu ứng phóng to Nút
using BeastLinkBattle.UI.MainMenu;

namespace BeastLinkBattle.UI.Views
{
    public class FooterTabView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _btnPetRoster;
        [SerializeField] private Button _btnMap;
        [SerializeField] private Button _btnShop;

        [Header("Selection Indicators (Borders)")]
        [Tooltip("Kéo thả GameObject viền (Border) nằm bên trong từng nút vào đây")]
        [SerializeField] private GameObject _rosterBorder;
        [SerializeField] private GameObject _mapBorder;
        [SerializeField] private GameObject _shopBorder;

        public event Action<MainMenuTab> OnTabClicked;
        private MainMenuTab? _currentTab = null;

        private void Awake()
        {
            _btnPetRoster.onClick.AddListener(() => OnTabClicked?.Invoke(MainMenuTab.PetRoster));
            _btnMap.onClick.AddListener(() => OnTabClicked?.Invoke(MainMenuTab.Map));
            _btnShop.onClick.AddListener(() => OnTabClicked?.Invoke(MainMenuTab.Shop));
        }

        public void HighlightTab(MainMenuTab tab)
        {
            // Tránh chạy lại logic nếu người chơi bấm liên tục vào tab đang chọn
            if (_currentTab == tab) return;

            // 1. Tắt tất cả các viền đang bật và reset scale về mặc định
            _rosterBorder.SetActive(false);
            _mapBorder.SetActive(false);
            _shopBorder.SetActive(false);

            _btnPetRoster.transform.localScale = Vector3.one;
            _btnMap.transform.localScale = Vector3.one;
            _btnShop.transform.localScale = Vector3.one;

            // 2. Bật viền của Tab được chọn và phóng to nó
            switch (tab)
            {
                case MainMenuTab.PetRoster:
                    _rosterBorder.SetActive(true);
                    _btnPetRoster.transform.DOScale(1.1f, 0.2f);
                    break;
                case MainMenuTab.Map:
                    _mapBorder.SetActive(true);
                    _btnMap.transform.DOScale(1.1f, 0.2f);
                    break;
                case MainMenuTab.Shop:
                    _shopBorder.SetActive(true);
                    _btnShop.transform.DOScale(1.1f, 0.2f);
                    break;
            }

            // 3. Cập nhật lại tab hiện tại
            _currentTab = tab;
        }
    }
}