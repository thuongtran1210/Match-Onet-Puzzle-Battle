// Scripts/Core/SystemTicker.cs
using UnityEngine;
using BeastLinkBattle.Gameplay;
using BeastLinkBattle.Gameplay.Battle;

namespace BeastLinkBattle.Core
{
    public class SystemTicker : MonoBehaviour
    {
        private IComboService _combo;
        private IEnemySpawnerService _spawner;
        private BattleSimulationService _sim;
        private bool _isInitialized;

        public bool IsBattleActive { get; set; } = false;

        public void Initialize(IComboService combo, IEnemySpawnerService spawner, BattleSimulationService sim)
        {
            _combo = combo;
            _spawner = spawner;
            _sim = sim;
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            float dt = Time.deltaTime;
            _combo?.Tick(dt);

            _spawner?.Tick(dt);

            _sim?.Tick(dt);
        }
    }
}