using UnityEngine;
using BeastLinkBattle.UI;
using BeastLinkBattle.UI.MainMenu;
using BeastLinkBattle.UI.Views;
using BeastLinkBattle.Services;

namespace BeastLinkBattle.UI.Presenters
{
    // MainMenuPresenter là một lớp chịu trách nhiệm quản lý giao diện chính của trò chơi, 
    // bao gồm việc hiển thị các tab khác nhau như Pet Roster, Map, và Shop. 
    // Nó lắng nghe sự kiện khi người chơi nhấp vào các tab trong FooterTabView và chuyển đổi giữa các tab tương ứng. 
    // Khi được hủy, nó sẽ hủy đăng ký sự kiện để tránh rò rỉ bộ nhớ.

    public class MainMenuPresenter : MonoBehaviour
    {
        [Header("UI Managers")]
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private FooterTabView _footerView;

        [Header("Tab Presenters")]
        [SerializeField] private PetRosterPresenter _petRosterTab;
        [SerializeField] private LevelSelectionPresenter _mapTab;
        [SerializeField] private ShopPresenter _shopTab;

        private IInventoryService _inventoryService;
        private ICurrencyService _currencyService;
        private IDeckSessionService _deckSessionService;
        public void Setup(IInventoryService inventoryService, ICurrencyService currencyService, IDeckSessionService deckSessionService)
        {
            _inventoryService = inventoryService;
            _currencyService = currencyService;
            _deckSessionService = deckSessionService;
        }
        public void Initialize()
        {
            _uiManager.HidePanel(PanelType.PreBattle);
            _uiManager.ShowPanel(PanelType.MainMenu, hideOthers: true);
            _footerView.OnTabClicked += SwitchTab;

            // Truyền service xuống các Tab con
            if (_petRosterTab != null)
                _petRosterTab.Setup(_inventoryService, _deckSessionService);

            if (_shopTab != null)
                _shopTab.Setup(_inventoryService, _currencyService);

            SwitchTab(MainMenuTab.Map);
        }

        private void SwitchTab(MainMenuTab tab)
        {
            _petRosterTab.gameObject.SetActive(false);
            _mapTab.gameObject.SetActive(false);
            _shopTab.gameObject.SetActive(false);

            switch (tab)
            {
                case MainMenuTab.PetRoster:
                    _petRosterTab.gameObject.SetActive(true);
                    _petRosterTab.Initialize();
                    _petRosterTab.ShowDefaultState();
                    _uiManager.ShowPanel(PanelType.SelectedDeck);
                    break;

                case MainMenuTab.Map:
                    _mapTab.gameObject.SetActive(true);
                    _mapTab.Initialize();
                    _uiManager.HidePanel(PanelType.SelectedDeck);
                    break;

                case MainMenuTab.Shop:
                    _shopTab.gameObject.SetActive(true);
                    _shopTab.Initialize();
                    _uiManager.HidePanel(PanelType.SelectedDeck);
                    break;
            }

            _footerView.HighlightTab(tab);
        }

        private void OnDestroy()
        {
            if (_footerView != null)
                _footerView.OnTabClicked -= SwitchTab;
        }
    }
}