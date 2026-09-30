using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine.UI;
using System;
using BeastLinkBattle.Gameplay.Data;

namespace BeastLinkBattle.UI.Views
{
    public class BattleQueueView : MonoBehaviour
    {
        [Header("Queue Icons Settings")]
        [SerializeField] private Transform _iconContainer;
        [SerializeField] private GameObject _beastIconPrefab;

        [Header("UI Buttons")]
        [SerializeField] private Button _startBattleBtn;

        public event Action OnStartBattleClicked;
        public event Action<BeastDefinition> OnBeastIconClicked;

        private ObjectPool<BeastIconView> _iconPool;
        private List<BeastIconView> _activeIcons = new List<BeastIconView>();

        private bool _isInitialized = false; 

        private void Awake()
        {
            InitializeIfNeeded();
        }
        private void InitializeIfNeeded()
        {
            if (_isInitialized) return;

            _iconPool = new ObjectPool<BeastIconView>(
                createFunc: () => Instantiate(_beastIconPrefab, _iconContainer).GetComponent<BeastIconView>(),
                actionOnGet: icon => icon.gameObject.SetActive(true),
                actionOnRelease: icon =>
                {
                    icon.gameObject.SetActive(false);
                    icon.transform.localScale = Vector3.one;
                },
                actionOnDestroy: icon => Destroy(icon.gameObject),
                defaultCapacity: 10,
                maxSize: 30
            );

            if (_startBattleBtn != null)
            {
                _startBattleBtn.onClick.AddListener(() => OnStartBattleClicked?.Invoke());
            }

            _isInitialized = true;
        }

        public void UpdateQueueDisplay(Dictionary<BeastDefinition, int> queueData)
        {
            
            InitializeIfNeeded();

            foreach (var icon in _activeIcons)
            {
                _iconPool.Release(icon);
            }
            _activeIcons.Clear();

            if (queueData == null || queueData.Count == 0) return;

            foreach (var kvp in queueData)
            {
                BeastDefinition def = kvp.Key;
                int count = kvp.Value;
                for (int i = 0; i < count; i++)
                {
                    BeastIconView iconView = _iconPool.Get();
                    iconView.Setup(def, def.uiColor, (clickedDef) => OnBeastIconClicked?.Invoke(clickedDef));
                    _activeIcons.Add(iconView);
                }
            }
        }
    }
}