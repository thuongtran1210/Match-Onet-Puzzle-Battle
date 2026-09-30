using BeastLinkBattle.Core;
using BeastLinkBattle.Gameplay.Battle;
using BeastLinkBattle.Grid.Manager;
using BeastLinkBattle.InputSystem;
using UnityEngine;

namespace BeastLinkBattle.Gameplay.GameState
{
    public class AutoBattleState : IGameState
    {
        private readonly GameBootstrapper _bootstrapper;
        private readonly BattleSimulationService _simulation;
        private readonly IEnemySpawnerService _spawner;
        private readonly IBattleService _battleService;
        private readonly ITileFactory _tileFactory; 
        private readonly IInputProvider _inputProvider;
        private readonly GameFlowManager _flowManager;
        private readonly WaveTransitionState _waveTransitionState;
        private float _stateTimer = 0f;
        private const float CHECK_DELAY = 0.5f;

        public AutoBattleState(GameBootstrapper bootstrapper, BattleSimulationService simulation, IEnemySpawnerService spawner, IBattleService battleService, ITileFactory tileFactory, IInputProvider inputProvider, GameFlowManager flowManager, WaveTransitionState waveTransitionState)
        {
            _bootstrapper = bootstrapper;
            _simulation = simulation;
            _spawner = spawner;
            _battleService = battleService;
            _tileFactory = tileFactory;
            _inputProvider = inputProvider;
            _flowManager = flowManager;
            _waveTransitionState = waveTransitionState;
        }

        public void Enter()
        {
            _stateTimer = 0f;
            _bootstrapper.SetBattleActive(true);
            _simulation.IsWaveActive = true;
            _inputProvider.EnableInput(true);
            _tileFactory.SetSpawnMode(SpawnMode.EnergyOnly);
            _bootstrapper.boardController.ClearSpecificContent(clearBeast: true);
            BeastLinkBattle.UI.UIManager.Instance.ShowPanel(BeastLinkBattle.UI.PanelType.MainHUD);
        }

        public void Update()
        {
            _stateTimer += Time.deltaTime;
            if (_stateTimer < CHECK_DELAY) return;

            bool isPlayerWipedOut = _simulation.ActivePlayerBeastsCount <= 0;
            bool isEnemyStillAliveOrSpawning = _simulation.GetAllEnemies().Count > 0 || !_spawner.IsReadyForNextWave();

            if (isPlayerWipedOut && isEnemyStillAliveOrSpawning)
            {
                Debug.Log("Bạn đã bị đánh bại! Chuyển sang màn hình Game Over.");
                _bootstrapper.TriggerGameOver(false, 0);
            }
            if (!isEnemyStillAliveOrSpawning)
            {
                if (_spawner.TryAdvanceToNextWave())
                {
                    Debug.Log("Wave Cleared.");
                    _flowManager.ChangeState(_waveTransitionState);
                }
                else
                {
                    Debug.Log("Bạn đã chiến thắng tất cả các đợt! Chuyển sang màn hình Victory.");
                    int earnedStars = 3;
                    _bootstrapper.TriggerGameOver(true, earnedStars);
                }
            }
        }

        public void Exit()
        {
            Debug.Log("--- END AUTO BATTLE ---");
            _bootstrapper.SetBattleActive(false);
            _simulation.IsWaveActive = false;
        }
    }
}