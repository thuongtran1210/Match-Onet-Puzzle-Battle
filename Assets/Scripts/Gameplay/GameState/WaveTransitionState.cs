// Scripts/Gameplay/GameState/WaveTransitionState.cs
using System;
using UnityEngine;
using BeastLinkBattle.Core;
using BeastLinkBattle.Gameplay.Battle;

namespace BeastLinkBattle.Gameplay.GameState
{
    // State chuyển tiếp giữa các wave, hiển thị thông tin về wave vừa hoàn thành và số thú còn sống sót
    public class WaveTransitionState : IGameState
    {
        private readonly GameBootstrapper _bootstrapper;
        private readonly BattleSimulationService _simulation;

        private int _clearedWaveCount = 1;
        private bool _isFinalWave = false;


        public event Action<int, int, Action> OnTransitionEntered;
        public event Action OnTransitionExited;

        public WaveTransitionState(GameBootstrapper bootstrapper, BattleSimulationService simulation)
        {
            _bootstrapper = bootstrapper;
            _simulation = simulation;
        }

        public void SetWaveData(int waveIndex) { _clearedWaveCount = waveIndex; }
        public void SetFinalWave(bool isFinal) { _isFinalWave = isFinal; }

        public void Enter()
        {
            _bootstrapper.SetBattleActive(false);
            Time.timeScale = 0f;

            int survived = _simulation.ActivePlayerBeastsCount;
            OnTransitionEntered?.Invoke(_clearedWaveCount, survived, ProceedToNextPhase);
        }

        public void Update() { }

        public void Exit()
        {
            Time.timeScale = 1f;
            OnTransitionExited?.Invoke();
        }

        private void ProceedToNextPhase()
        {
            if (_isFinalWave) _bootstrapper.TriggerGameOver(true, 3);
            else
            {
                _bootstrapper.FinishBattlePhase();
                _clearedWaveCount++;
            }
        }
    }
}