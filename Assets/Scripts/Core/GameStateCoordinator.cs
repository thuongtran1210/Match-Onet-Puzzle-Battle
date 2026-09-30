using System;
using UnityEngine;
using BeastLinkBattle.Gameplay;
using BeastLinkBattle.Gameplay.Battle;
using BeastLinkBattle.Gameplay.GameState;
using BeastLinkBattle.InputSystem;
using BeastLinkBattle.Grid.Controller;
using BeastLinkBattle.UI;

namespace BeastLinkBattle.Core.Coordinators
{
    public class GameStateCoordinator : IDisposable
    {
        private readonly GameFlowManager _flowManager;
        private readonly IComboService _comboService;
        private readonly BoardController _boardController;
        private readonly IInputProvider _inputProvider;
        private readonly IUIFlowService _uiFlow;

        private readonly PlayingState _playingState;
        private readonly ComboTimeState _comboTimeState;
        private readonly ResolutionState _resolutionState;
        private readonly PauseState _pauseState;
        private readonly WaveTransitionState _waveTransitionState;
        private readonly GameOverState _gameOverState;

        public event Action<int, int, Action> OnWaveTransitionUIRequested;
        public event Action<bool, int> OnGameOverUIRequested;

        public GameStateCoordinator(
            GameFlowManager flowManager, IComboService comboService, BoardController boardController,
            IInputProvider inputProvider, IUIFlowService uiFlow, PlayingState playingState,
            ComboTimeState comboTimeState, ResolutionState resolutionState, PauseState pauseState,
            WaveTransitionState waveTransitionState, GameOverState gameOverState)
        {
            _flowManager = flowManager;
            _comboService = comboService;
            _boardController = boardController;
            _inputProvider = inputProvider;
            _uiFlow = uiFlow;
            _playingState = playingState;
            _comboTimeState = comboTimeState;
            _resolutionState = resolutionState;
            _pauseState = pauseState;
            _waveTransitionState = waveTransitionState;
            _gameOverState = gameOverState;
        }

        public void Initialize()
        {
            _comboService.OnComboUpdated += HandleComboUpdated;
            _comboService.OnComboEnded += HandleComboEnded;

            if (_pauseState != null)
            {
                _pauseState.OnPauseEntered += () => _uiFlow.ShowPanel(PanelType.Pause, hideOthers: true);
                _pauseState.OnPauseExited += () => _uiFlow.HidePanel(PanelType.Pause);
            }

            if (_waveTransitionState != null)
            {
                _waveTransitionState.OnTransitionEntered += HandleWaveTransitionEntered;
                _waveTransitionState.OnTransitionExited += () => _uiFlow.HidePanel(PanelType.WaveTransition);
            }

            if (_gameOverState != null)
            {
                _gameOverState.OnGameOverEntered += HandleGameOverEntered;
                _gameOverState.OnGameOverExited += () => _uiFlow.HidePanel(PanelType.GameOver);
            }
        }

        private void HandleComboUpdated(int currentCombo)
        {
            if (_flowManager.CurrentState == _playingState) _flowManager.ChangeState(_comboTimeState);
        }

        private void HandleComboEnded()
        {
            _boardController.TriggerComboRespawn();
            if (_flowManager.CurrentState == _comboTimeState || _flowManager.CurrentState == _playingState)
            {
                _inputProvider.EnableInput(false);
                _flowManager.ChangeState(_resolutionState);
            }
        }

        private void HandleWaveTransitionEntered(int waveIndex, int survivedCount, Action proceedCallback)
        {
            _uiFlow.HidePanel(PanelType.MainHUD);
            _uiFlow.ShowPanel(PanelType.WaveTransition, hideOthers: true);
            OnWaveTransitionUIRequested?.Invoke(waveIndex, survivedCount, proceedCallback);
        }

        private void HandleGameOverEntered(bool isWin, int stars)
        {
            _uiFlow.ShowPanel(PanelType.GameOver, hideOthers: true);
            OnGameOverUIRequested?.Invoke(isWin, stars);
        }

        public void Dispose()
        {
            if (_comboService != null)
            {
                _comboService.OnComboUpdated -= HandleComboUpdated;
                _comboService.OnComboEnded -= HandleComboEnded;
            }
        }
    }
}