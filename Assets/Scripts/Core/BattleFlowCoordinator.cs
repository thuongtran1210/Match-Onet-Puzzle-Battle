using System.Collections;
using UnityEngine;
using BeastLinkBattle.Gameplay;
using BeastLinkBattle.Gameplay.Battle;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.GameState;
using BeastLinkBattle.Grid.Controller;
using BeastLinkBattle.InputSystem;
using BeastLinkBattle.UI;

namespace BeastLinkBattle.Core.Coordinators
{
    public class BattleFlowCoordinator : System.IDisposable
    {
        private readonly IBattleService _battleService;
        private readonly BattleSimulationService _battleSimulation;
        private readonly IEnemySpawnerService _enemySpawner;
        private readonly IComboService _comboService;
        private readonly GameFlowManager _flowManager;
        private readonly PlayingState _playingState;
        private readonly AutoBattleState _autoBattleState;
        private readonly IInputProvider _inputProvider;
        private readonly BoardController _boardController;
        private readonly IUIFlowService _uiFlow;

        public BattleFlowCoordinator(
            IBattleService battleService, BattleSimulationService battleSimulation,
            IEnemySpawnerService enemySpawner, IComboService comboService, GameFlowManager flowManager,
            PlayingState playingState, AutoBattleState autoBattleState, IInputProvider inputProvider,
            BoardController boardController, IUIFlowService uiFlow)
        {
            _battleService = battleService;
            _battleSimulation = battleSimulation;
            _enemySpawner = enemySpawner;
            _comboService = comboService;
            _flowManager = flowManager;
            _playingState = playingState;
            _autoBattleState = autoBattleState;
            _inputProvider = inputProvider;
            _boardController = boardController;
            _uiFlow = uiFlow;
        }

        public void Initialize()
        {
            _battleSimulation.OnEnemyDied += HandleEnemyDied;
        }

        private void HandleEnemyDied() => _enemySpawner.RegisterEnemyDeath();

        public void ResolveMatchQueue()
        {
            _battleService.ResolveBattlePhase();
            if (_comboService != null && _comboService.IsActive) _comboService.ForceEndCombo();

            if (_flowManager != null && _autoBattleState != null)
                _flowManager.ChangeState(_autoBattleState);

            _uiFlow.HidePanel(PanelType.BattleQueueHUD);
        }

        public IEnumerator FinishBattleRoutine()
        {
            if (_comboService != null && _comboService.IsActive) _comboService.ForceEndCombo();

            _flowManager.ChangeState(_playingState);
            _inputProvider.EnableInput(false);
            _boardController.ClearSpecificContent(clearBeast: false);

            yield return new WaitForSeconds(1.0f);

            _inputProvider.EnableInput(true);
        }

        public void SpawnSingleBeast(BeastDefinition def) => _battleService.ResolveSingleBeast(def);

        public void Dispose()
        {
            if (_battleSimulation != null) _battleSimulation.OnEnemyDied -= HandleEnemyDied;
        }
    }
}