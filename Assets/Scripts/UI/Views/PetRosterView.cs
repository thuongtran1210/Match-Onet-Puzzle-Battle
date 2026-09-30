using UnityEngine;
using UnityEngine.UI; // Cần thêm thư viện này để sử dụng Button
using System;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Grid.Data;

namespace BeastLinkBattle.UI.Views
{
    public class PetRosterView : MonoBehaviour
    {
        [Header("Tab Buttons")]
        [SerializeField] private Button _beastTabButton;
        [SerializeField] private Button _petTabButton;
        [SerializeField] private Button _energyTabButton;

        [Header("UI Containers")]
        [SerializeField] private Transform _petListContainer;
        [SerializeField] private Transform _beastListContainer;
        [SerializeField] private Transform _energyListContainer;

        [Header("Sub-Tab Panels (ScrollViews)")]
        [SerializeField] private GameObject _petScrollView;
        [SerializeField] private GameObject _beastScrollView;
        [SerializeField] private GameObject _energyScrollView;
        [SerializeField] private SelectedDeckView _selectedDeckView;

        [Header("Prefabs")]
        [SerializeField] private GameObject _petButtonPrefab;
        [SerializeField] private GameObject _beastTogglePrefab;
        [SerializeField] private GameObject _energyTogglePrefab;
        [SerializeField] private EnergyUIConfig _energyUiConfig;

        public event Action<BasePetDefinition> OnPetClicked;
        public event Action<BeastDefinition> OnBeastSelected;
        public event Action<BeastDefinition> OnBeastDeselected;
        public event Action<EnergyDefinition> OnEnergyDeselected;
        public event Action<EnergyDefinition> OnEnergySelected;


        private List<BeastToggleView> _beastToggleViews = new List<BeastToggleView>();
        private List<EnergyToggleView> _energyToggleViews = new List<EnergyToggleView>();
        private List<PetToggleView> _petToggleViews = new List<PetToggleView>();

        private void Awake()
        {
            // Đăng ký sự kiện click cho các nút Tab
            if (_beastTabButton != null) _beastTabButton.onClick.AddListener(ShowSubTabBeast);
            if (_petTabButton != null) _petTabButton.onClick.AddListener(ShowSubTabPet);
            if (_energyTabButton != null) _energyTabButton.onClick.AddListener(ShowSubTabEnergy);
        }

        private void OnDestroy()
        {
            // Huỷ đăng ký sự kiện để tránh rò rỉ bộ nhớ
            if (_beastTabButton != null) _beastTabButton.onClick.RemoveListener(ShowSubTabBeast);
            if (_petTabButton != null) _petTabButton.onClick.RemoveListener(ShowSubTabPet);
            if (_energyTabButton != null) _energyTabButton.onClick.RemoveListener(ShowSubTabEnergy);
        }

        public void ShowLoadingState() { }
        public void Hide()
        {
            // Tắt SelectedDeckView
            if (_selectedDeckView != null)
            {
                _selectedDeckView.gameObject.SetActive(false);
            }

            // Tắt chính nó (nếu PetRosterView là một Panel riêng biệt)
            gameObject.SetActive(false);
        }

        public void ShowDefaultState()
        {
            // 1. Hiển thị SelectedDeckView (Nếu nó là con của PetRosterView)
            if (_selectedDeckView != null)
            {
                _selectedDeckView.gameObject.SetActive(true);
            }

            // 2. Chỉ bật danh sách Beast, tắt các danh sách còn lại
            ShowSubTabBeast();
        }

        public void ShowSubTabBeast()
        {
            if (_beastScrollView != null) _beastScrollView.SetActive(true);
            if (_petScrollView != null) _petScrollView.SetActive(false);
            if (_energyScrollView != null) _energyScrollView.SetActive(false);
        }

        public void ShowSubTabPet()
        {
            if (_beastScrollView != null) _beastScrollView.SetActive(false);
            if (_petScrollView != null) _petScrollView.SetActive(true);
            if (_energyScrollView != null) _energyScrollView.SetActive(false);
        }

        public void ShowSubTabEnergy()
        {
            if (_beastScrollView != null) _beastScrollView.SetActive(false);
            if (_petScrollView != null) _petScrollView.SetActive(false);
            if (_energyScrollView != null) _energyScrollView.SetActive(true);
        }

        public void PopulatePetRoster(IEnumerable<BasePetDefinition> pets)
        {
            if (_petListContainer == null) return;
            foreach (Transform child in _petListContainer) Destroy(child.gameObject);

            _petToggleViews.Clear();

            foreach (var pet in pets)
            {
                var obj = Instantiate(_petButtonPrefab, _petListContainer); // Tên prefab
                var view = obj.GetComponent<PetToggleView>();

                view?.Setup(pet,
                    onSelect: (p) => OnPetClicked?.Invoke(p),
                    onDeselect: null
                );

                // Đảm bảo truyền 'view' vào Add()
                if (view != null) _petToggleViews.Add(view);
            }
        }

        public void PopulateBeastRoster(IEnumerable<BeastDefinition> beasts)
        {
            if (_beastListContainer == null) return;
            foreach (Transform child in _beastListContainer) Destroy(child.gameObject);

            _beastToggleViews.Clear();

            foreach (var beast in beasts)
            {
                var obj = Instantiate(_beastTogglePrefab, _beastListContainer);
                var view = obj.GetComponent<BeastToggleView>();
                view?.Setup(beast,
                    onSelect: (b) => OnBeastSelected?.Invoke(b),
                    onDeselect: (b) => OnBeastDeselected?.Invoke(b));

                if (view != null) _beastToggleViews.Add(view);
            }
        }

        // Note: PopulateEnergyRoster uses an IEnumerable of EnergyType but EnergyToggleView takes EnergyDefinition now.
        // Assuming you handle this discrepancy via fetching definition by type elsewhere, or you may need to update this to take definitions instead.
        public void PopulateEnergyRoster(IEnumerable<EnergyDefinition> energies) // Changed to EnergyDefinition
        {
            if (_energyListContainer == null) return;
            foreach (Transform child in _energyListContainer) Destroy(child.gameObject);

            _energyToggleViews.Clear();

            foreach (var energy in energies)
            {
                var obj = Instantiate(_energyTogglePrefab, _energyListContainer);
                var view = obj.GetComponent<EnergyToggleView>();

                // Assuming EnergyToggleView.Setup was updated as we did previously.
                view?.Setup(energy,
                    onSelect: (e) => OnEnergySelected?.Invoke(e),
                    onDeselect: (e) => OnEnergyDeselected?.Invoke(e));

                if (view != null) _energyToggleViews.Add(view);
            }
        }

        public void SyncToggles(BasePetDefinition savedPet, List<BeastDefinition> savedBeasts, List<EnergyDefinition> savedEnergies)
        {
            if (savedBeasts != null)
            {
                foreach (var view in _beastToggleViews)
                {
                    if (savedBeasts.Contains(view.MyDef))
                    {
                        view.ForceSelectWithoutNotify();
                    }
                }
            }

            if (savedEnergies != null)
            {
                foreach (var view in _energyToggleViews)
                {
                    // So sánh trực tiếp Definition thay vì so sánh enum bên trong
                    if (view.MyType != null && savedEnergies.Contains(view.MyType))
                    {
                        view.ForceSelectWithoutNotify();
                    }
                    else
                    {
                        view.ForceDeselectWithoutNotify(); // Nên có để clear trạng thái cũ
                    }
                }
            }

            if (savedPet != null)
            {
                foreach (var view in _petToggleViews) // _petToggleViews là List<PetToggleView>
                {
                    if (view.MyDef == savedPet)
                    {
                        view.ForceSelectWithoutNotify();
                    }
                    else
                    {
                        // Bắt buộc tắt viền các con Pet khác để đảm bảo chỉ có 1 con được chọn
                        view.ForceDeselectWithoutNotify();
                    }
                }
            }
        }
    }
}