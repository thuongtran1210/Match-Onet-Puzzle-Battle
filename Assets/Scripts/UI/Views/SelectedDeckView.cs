using UnityEngine;
using System;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Data;
using Unity.VisualScripting;

namespace BeastLinkBattle.UI.Views
{
    public class SelectedDeckView : MonoBehaviour
    {
        [Header("Containers")]
        [SerializeField] private Transform _leaderPetContainer;
        [SerializeField] private Transform _beastListContainer;
        [SerializeField] private Transform _energyListContainer;

        [Header("Prefabs")]
        [SerializeField] private GameObject _petItemPrefab;
        [SerializeField] private GameObject _beastItemPrefab;
        [SerializeField] private GameObject _energyItemPrefab;

        public event Action<BeastDefinition> OnBeastSelectedForDeploy;

        private List<GameObject> _spawnedItems = new List<GameObject>();

        public void DisplayDeck(PlayerDeck deck)
        {
            ClearContainers();

            if (deck == null) return;

            // 1. Hiển thị Leader Pet (Không cần click)
            if (deck.LeaderPet != null)
            {
                var petObj = Instantiate(_petItemPrefab, _leaderPetContainer);
                var itemView = petObj.GetComponent<DeckItemView>();
                if (itemView != null)
                {
                    // Giả sử BasePetDefinition có trường 'icon'
                    itemView.Setup(deck.LeaderPet.uiIcon, Color.white, null);
                }
                _spawnedItems.Add(petObj);
            }

            // 2. Hiển thị danh sách Beast (Cần click để Manual Deploy)
            if (deck.SelectedBeasts != null)
            {
                foreach (var beast in deck.SelectedBeasts)
                {
                    var beastObj = Instantiate(_beastItemPrefab, _beastListContainer);
                    var itemView = beastObj.GetComponent<DeckItemView>();

                    if (itemView != null)
                    {
                        // Giả sử BeastDefinition có 'icon' và 'uiColor'
                        itemView.Setup(beast.uiIcon, beast.uiColor, () => HandleBeastSelection(beast));
                    }
                    _spawnedItems.Add(beastObj);
                }
            }

            // 3. Hiển thị danh sách Energy (Không cần click)
            if (deck.SelectedEnergies != null)
            {
                foreach (var energy in deck.SelectedEnergies)
                {
                    var energyObj = Instantiate(_energyItemPrefab, _energyListContainer);
                    var itemView = energyObj.GetComponent<DeckItemView>();

                    if (itemView != null)
                    {
                        // Giả sử EnergyDefinition có 'icon'
                        itemView.Setup(energy.uiIcon, Color.white, null);
                    }
                    _spawnedItems.Add(energyObj);
                }
            }
        }

        private void HandleBeastSelection(BeastDefinition selectedBeast)
        {
            // Bắn tín hiệu chuẩn bị triển khai thú
            OnBeastSelectedForDeploy?.Invoke(selectedBeast);
        }

        private void ClearContainers()
        {
            foreach (var item in _spawnedItems)
            {
                if (item != null) Destroy(item);
            }
            _spawnedItems.Clear();
        }
    }
}