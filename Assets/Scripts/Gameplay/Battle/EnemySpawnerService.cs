using System;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Battle.Data;

namespace BeastLinkBattle.Gameplay.Battle
{
    public class EnemySpawnerService : IEnemySpawnerService
    {
        public event Action<EnemyDefinition, int, bool> OnEnemySpawnRequested;
        public event Action OnWaveWarning;
        public event Action<int, int> OnWaveChanged;

        private List<WaveData> _waves;
        private int _currentWaveIndex;
        private float _waveTimer;
        private bool _isActive;

        private float _spawnTimer;
        private int _currentConfigIndex;
        private int _spawnedCountInConfig;
        private int _aliveEnemiesCount;

        public void StartLevel(List<WaveData> waves)
        {
            if (waves == null || waves.Count == 0) return;
            _waves = waves;
            _currentWaveIndex = 0;
            _aliveEnemiesCount = 0;
            _isActive = true;

            SetupCurrentWave();
            OnWaveChanged?.Invoke(_currentWaveIndex + 1, _waves.Count);
        }

        private void SetupCurrentWave()
        {
            _currentConfigIndex = 0;
            _spawnedCountInConfig = 0;
            _spawnTimer = 0f;

            if (_currentWaveIndex < _waves.Count)
            {
                var wave = _waves[_currentWaveIndex];
                _waveTimer = wave.timeToNextWave;

                foreach (var cfg in wave.enemies)
                {
                    if (cfg.isMiniBoss)
                    {
                        OnWaveWarning?.Invoke();
                        break;
                    }
                }
            }
            else
            {
                _isActive = false; 
            }
        }

        public void Tick(float deltaTime)
        {
            if (!_isActive) return;
            var currentWave = _waves[_currentWaveIndex];
            if (_currentConfigIndex < currentWave.enemies.Count)
            {
                var currentConfig = currentWave.enemies[_currentConfigIndex];
                _spawnTimer -= deltaTime;

                if (_spawnTimer <= 0f)
                {
                    OnEnemySpawnRequested?.Invoke(currentConfig.definition, currentConfig.starLevel, currentConfig.isMiniBoss);
                    _spawnedCountInConfig++;
                    _aliveEnemiesCount++;
                    _spawnTimer = currentConfig.delayBetweenSpawns;
                    if (_spawnedCountInConfig >= currentConfig.count)
                    {
                        _currentConfigIndex++;
                        _spawnedCountInConfig = 0;
                        _spawnTimer = 0f; 
                    }
                }
            }
            else
            {
                if (!currentWave.waitForClear && _waveTimer > 0)
                {
                    _waveTimer -= deltaTime;
                }
            }
        }
        public bool IsReadyForNextWave()
        {
            var currentWave = _waves[_currentWaveIndex];
            bool allSpawned = _currentConfigIndex >= currentWave.enemies.Count;
            if (currentWave.waitForClear)
            {
                return allSpawned && _aliveEnemiesCount <= 0;
            }
            else
            {
                return allSpawned && _waveTimer <= 0;
            }
        }

        public bool TryAdvanceToNextWave()
        {
            if (_currentWaveIndex + 1 < _waves.Count)
            {
                _currentWaveIndex++;
                SetupCurrentWave();
                OnWaveChanged?.Invoke(_currentWaveIndex + 1, _waves.Count);
                return true;
            }
            return false; 
        }


        public void RegisterEnemyDeath()
        {
            _aliveEnemiesCount--;
            if (_aliveEnemiesCount < 0) _aliveEnemiesCount = 0;
        }

        private void AdvanceToNextWave()
        {
            _currentWaveIndex++;
            SetupCurrentWave();
        }
    }
}