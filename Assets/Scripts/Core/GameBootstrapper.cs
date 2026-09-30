// Scripts/Core/GameBootstrapper.cs
using UnityEngine;
using System.Collections.Generic;
using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Grid.Manager;
using BeastLinkBattle.Grid.Service;
using BeastLinkBattle.Grid.Service.Rules;
using BeastLinkBattle.Grid.Controller;
using BeastLinkBattle.InputSystem;
using BeastLinkBattle.Gameplay.Level;
using BeastLinkBattle.Grid.View;
using BeastLinkBattle.Gameplay;
using BeastLinkBattle.UI.Views;
using BeastLinkBattle.Gameplay.Battle;
using BeastLinkBattle.Gameplay.GameState;
using BeastLinkBattle.Gameplay.Data;
using DG.Tweening;
using BeastLinkBattle.UI.Presenters;
using BeastLinkBattle.UI;
using BeastLinkBattle.Services;
using BeastLinkBattle.Core.Coordinators;

namespace BeastLinkBattle.Core
{
    [RequireComponent(typeof(GameFlowManager), typeof(SystemTicker))]
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Global Services")]
        [SerializeField] private LocalInventoryService localInventoryService;
        private IInventoryService _inventoryService;
        [SerializeField] private LocalCurrencyService localCurrencyService;
        private ICurrencyService _currencyService;

        [Header("Global Presenters")]
        [SerializeField] private PreBattlePresenter preBattlePresenter;
        [SerializeField] private SelectedDeckView selectedDeckView;
        [SerializeField] private MainMenuPresenter mainMenuPresenter;
        [SerializeField] private PetRosterPresenter petRosterPresenter;

        [Header("References")]
        public BoardController boardController;
        [SerializeField] private NewInputSystemProvider inputProvider;
        [SerializeField] private UIManager uiManager;

        [Header("Configurations")]
        public LevelData currentLevel;
        public BeastBattleConfig beastBattleLogicConfig;
        public float tileSize = 1.1f;
        private PlayerDeck _currentDeck;

        [Header("Gameplay Services Config")]
        public EnergyConfig energyConfig;
        public ComboConfig comboConfig;

        [Header("UI Views (Passive Views)")]
        [SerializeField] private GameplayViewContainer _views;
        [SerializeField] private HeaderView _headerView;

        // --- MANAGER ---
        private GameFlowManager _flowManager;
        private SystemTicker _systemTicker;
        private GridFlowCoordinator _gridCoordinator;
        private GameStateCoordinator _stateCoordinator;
        private BattleFlowCoordinator _battleCoordinator;
        private SelectionController _selectionController;

        // --- SERVICES ---
        private IBattleService _battleService;
        private IEnemySpawnerService _enemySpawnerService;
        private IDeckSessionService _deckSessionService;

        [Header("--- DEBUG DECK SESSION ---")]
        [SerializeField] private string debugLeaderPet;
        [SerializeField] private int debugSelectedBeastCount;
        [SerializeField] private int debugSelectedEnergyCount;

        // --- GAME STATES ---
        private AutoBattleState _autoBattleState;
        private WaveTransitionState _waveTransitionState;
        private GameOverState _gameOverState;
        private BeastLinkBattle.Gameplay.GameState.PauseState _pauseState;

        // --- PRESENTER ---
        private EnergyPresenter _energyPresenter;
        private ComboPresenter _comboPresenter;
        private WavePresenter _wavePresenter;
        private TeamHealthPresenter _teamHealthPresenter;
        private BattleQueuePresenter _battleQueuePresenter;
        private GameOverPresenter _gameOverPresenter;
        private WaveClearPresenter _waveClearPresenter;
        private CurrencyPresenter _currencyPresenter;
        private HeaderPresenter _headerPresenter;
        private SelectedDeckPresenter _selectedDeckPresenter;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly);

            if (localInventoryService != null && localCurrencyService != null)
            {
                // 1. Khởi tạo Inventory & Currency Service
                _inventoryService = localInventoryService;
                _inventoryService.Initialize();
                //Debug.Log("Inventory Service Initialized!");

                _currencyService = localCurrencyService;
                _currencyService.Initialize();
                //Debug.Log("Currency Service Initialized!");
                _headerPresenter = new HeaderPresenter(_headerView, _currencyService);

                // ==========================================================
                // 2. KHỞI TẠO DECK SESSION SERVICE (SINGLE SOURCE OF TRUTH)
                // ==========================================================
                // Tạo Save System (được dùng chung cho SelectedDeckPresenter cũ và DeckSession mới)
                IDeckSaveSystem deckSaveSystem = new JsonDeckSaveSystem(_inventoryService);

                // Khởi tạo DeckSessionService và Inject các dependency của nó vào
                _deckSessionService = new DeckSessionService(deckSaveSystem, _inventoryService);
                _deckSessionService.Initialize();
                //Debug.Log("Deck Session Service Initialized!");
                _deckSessionService.OnDeckChanged += UpdateDeckDebugView;
                UpdateDeckDebugView();

                // ==========================================================
                // 3. INJECT VÀO PRE-BATTLE PRESENTER
                // ==========================================================
                if (preBattlePresenter != null)
                {
                    // Inject cả 2 services vào Setup
                    preBattlePresenter.Setup(_inventoryService, _deckSessionService);
                    preBattlePresenter.Initialize();
                    //Debug.Log("PreBattlePresenter Initialized!");
                }
                else
                {
                    Debug.LogWarning("Chưa gán PreBattlePresenter vào GameBootstrapper!");
                }
                // ==========================================================
                // 3. INJECT VÀO MAIN MENU PRESENTER
                // ==========================================================
                if (mainMenuPresenter != null)
                {
                    mainMenuPresenter.Setup(_inventoryService, _currencyService, _deckSessionService);
                    mainMenuPresenter.Initialize();
                    //Debug.Log("MainMenuPresenter Initialized!");
                }
                // ==========================================================
                // 4. INJECT VÀO PET ROSTER PRESENTER
                // ==========================================================
                if (petRosterPresenter != null)
                {
                    // Inject cả 2 services vào Setup
                    petRosterPresenter.Setup(_inventoryService, _deckSessionService);
                    petRosterPresenter.Initialize();
                    //Debug.Log("PetRosterPresenter Initialized!");
                }
                else
                {
                    Debug.LogWarning("Chưa gán PetRosterPresenter vào GameBootstrapper!");
                }

                // ==========================================================
                // 5. Setup SelectedDeckPresenter (Giữ nguyên logic cũ của bạn)
                // ==========================================================
                if (selectedDeckView != null)
                {
                    _selectedDeckPresenter = new SelectedDeckPresenter(selectedDeckView, deckSaveSystem);
                    _selectedDeckPresenter.Initialize();
                    //Debug.Log("SelectedDeckPresenter Initialized!");
                }
                else
                {
                    Debug.LogWarning("Chưa gán SelectedDeckView vào GameBootstrapper!");
                }
            }
            else
            {
                Debug.LogError("Chưa gán LocalInventoryService hoặc LocalCurrencyService vào GameBootstrapper!");
            }
        }
        private void Start()
        {
            UIManager.Instance.ShowPanel(PanelType.Header);
        }
        public void StartGame(LevelData selectedLevel, PlayerDeck deck)
        {
            this.currentLevel = selectedLevel;
            this._currentDeck = deck;

            _flowManager = GetComponent<GameFlowManager>();
            _systemTicker = GetComponent<SystemTicker>();

            // 1. GRID SERVICES 
            IGridService gridService = new GridService(currentLevel.shape);
            ITileFactory tileFactory = new TileFactory(_currentDeck);
            IMatchService matchService = new MatchService(gridService, new List<IMatchRule> { new StandardOnetMatchRule() });
            IRespawnService respawnService = new RespawnService(gridService, tileFactory);
            IGridManager gridManager = new GridManager(tileFactory, gridService);
            IShuffleService shuffleService = new ShuffleService(gridService, matchService);

            tileFactory.SetSpawnMode(SpawnMode.BeastOnly);
            boardController.Initialize(gridService, matchService, respawnService, gridManager, currentLevel.respawnPolicy, shuffleService);

            // 2. BATTLE SERVICES 
            EntityFactory entityFactory = new EntityFactory(beastBattleLogicConfig, _currentDeck.LeaderPet);
            BattleSimulationService battleSimulation = new BattleSimulationService();
            _enemySpawnerService = new EnemySpawnerService();
            IEnergyService energyService = new EnergyManager(energyConfig);
            IComboService comboService = new ComboManager(comboConfig);

            Vector3 playerSpawn = _views.battlefieldView != null ? _views.battlefieldView.PlayerSpawnPos : Vector3.zero;
            Vector3 enemySpawn = _views.battlefieldView != null ? _views.battlefieldView.EnemySpawnPos : Vector3.zero;

            _battleService = new BattleManager(battleSimulation, entityFactory, _enemySpawnerService, playerSpawn, enemySpawn, beastBattleLogicConfig, _currentDeck.LeaderPet);
            // 3. KẾT NỐI VỚI VIEW & CONTROLLER


            // Các view nào cần setup với service thì setup ở đây, tránh việc service phụ thuộc vào view (VD: battleService không nên biết battlefieldView tồn tại)
            _views.battlefieldView?.Setup(_battleService, _enemySpawnerService);
           
            _selectionController = new SelectionController(inputProvider, gridService, tileSize, boardController.transform.position);
            _views.selectionView?.Setup(_selectionController, boardController);

            // 4. STATES & MEDIATOR
            PlayingState playingState = new PlayingState(inputProvider, tileFactory);
            ComboTimeState comboTimeState = new ComboTimeState();
            _gameOverState = new GameOverState();
            _waveTransitionState = new WaveTransitionState(this, battleSimulation);
            _pauseState = new BeastLinkBattle.Gameplay.GameState.PauseState(inputProvider);

            _autoBattleState = new AutoBattleState(this, battleSimulation, _enemySpawnerService, _battleService, tileFactory, inputProvider, _flowManager, _waveTransitionState);
            ResolutionState resolutionState = new ResolutionState(this, _flowManager, _autoBattleState);

            IUIFlowService uiFlow = uiManager != null ? uiManager : UIManager.Instance;

            // Khởi tạo các Coordinators thay cho God Mediator
            _gridCoordinator = new GridFlowCoordinator(boardController, _selectionController, energyService, _battleService, comboService);
            _stateCoordinator = new GameStateCoordinator(_flowManager, comboService, boardController, inputProvider, uiFlow, playingState, comboTimeState, resolutionState, _pauseState, _waveTransitionState, _gameOverState);
            _battleCoordinator = new BattleFlowCoordinator(_battleService, battleSimulation, _enemySpawnerService, comboService, _flowManager, playingState, _autoBattleState, inputProvider, boardController, uiFlow);

            // Bind Events
            _gridCoordinator.Initialize();
            _stateCoordinator.Initialize();
            _battleCoordinator.Initialize();

            BindUIPresenters(energyService, comboService, _enemySpawnerService, _battleService, _stateCoordinator, uiFlow);

            // 5. KHỚI ĐỘNG TRÒ CHƠI
            _systemTicker.Initialize(comboService, _enemySpawnerService, battleSimulation);
            _enemySpawnerService.StartLevel(currentLevel.waves);
            _flowManager.ChangeState(playingState);
        }

        private void UpdateDeckDebugView()
        {
            if (_deckSessionService == null || _deckSessionService.CurrentDeck == null) return;

            var currentDeck = _deckSessionService.CurrentDeck;

            debugLeaderPet = currentDeck.LeaderPet != null ? currentDeck.LeaderPet.name : "None";
            debugSelectedBeastCount = currentDeck.SelectedBeasts != null ? currentDeck.SelectedBeasts.Count : 0;
            debugSelectedEnergyCount = currentDeck.SelectedEnergies != null ? currentDeck.SelectedEnergies.Count : 0;
        }
        private void BindUIPresenters(IEnergyService energyService, IComboService comboService, IEnemySpawnerService spawnerService, IBattleService battleService, GameStateCoordinator stateCoordinator, IUIFlowService uiFlow)
        {
            // Reset các presenter cũ nếu có (tránh trường hợp StartGame được gọi nhiều lần dẫn đến việc tạo nhiều presenter mới mà không dispose presenter cũ)
            DisposePresenters();

            if (_views.energyView != null)
                _energyPresenter = new EnergyPresenter(_views.energyView, energyService, battleService);

            if (_views.comboView != null)
                _comboPresenter = new ComboPresenter(_views.comboView, comboService);

            if (_views.waveView != null)
                _wavePresenter = new WavePresenter(_views.waveView, spawnerService);

            if (_views.teamHealthView != null)
                _teamHealthPresenter = new TeamHealthPresenter(_views.teamHealthView, battleService);

            if (_views.battleQueueView != null)
                _battleQueuePresenter = new BattleQueuePresenter(_views.battleQueueView, battleService, this);

            if (_views.GameOverView != null)
                _gameOverPresenter = new GameOverPresenter(_views.GameOverView, this, stateCoordinator);

            if (_views.WaveClearView != null)
                _waveClearPresenter = new WaveClearPresenter(_views.WaveClearView, this, stateCoordinator, uiFlow);
        }
   
        public void OpenPreBattle(LevelData selectedLevel)
        {
            if (preBattlePresenter != null)
            {
                // 1. Truyền thông tin Level (để nó load danh sách quái và validate)
                preBattlePresenter.SetSelectedLevel(selectedLevel);

                // 2. Mở UI hiển thị màn hình PreBattle
                if (uiManager != null)
                {
                    uiManager.ShowPanel(PanelType.PreBattle, hideOthers: true);
                }
            }
        }

        // --- CÁC HÀM BRIDGE  ---
        public void StartManualBattle()
        {
            if (_flowManager != null && _flowManager.CurrentState is ResolutionState)
                _flowManager.ChangeState(_autoBattleState);
            else
                Debug.LogWarning("Chua x?p quân xong, không th? b?t d?u dánh!");
        }

        public void ResolveMatchQueue() => _battleCoordinator.ResolveMatchQueue();
        public void FinishBattlePhase() => StartCoroutine(_battleCoordinator.FinishBattleRoutine());
        public void SpawnSingleBeast(BeastDefinition def) => _battleCoordinator.SpawnSingleBeast(def);
        public void SetBattleActive(bool isActive) => _systemTicker.IsBattleActive = isActive;

        public void TriggerGameOver(bool isWin, int stars)
        {
            if (_gameOverState != null)
            {
                _gameOverState.SetResult(isWin, stars);
                _flowManager.ChangeState(_gameOverState);
            }
        }

        private void DisposePresenters()
        {
            _energyPresenter?.Dispose();
            _comboPresenter?.Dispose();
            _wavePresenter?.Dispose();
            _teamHealthPresenter?.Dispose();
            _battleQueuePresenter?.Dispose();
            _gameOverPresenter?.Dispose();
            _waveClearPresenter?.Dispose();
        }

        private void OnDestroy()
        {
            _selectionController?.Dispose();
            _gridCoordinator?.Dispose();
            _stateCoordinator?.Dispose();
            _battleCoordinator?.Dispose();
            DisposePresenters();
            _headerPresenter?.Dispose();
            _selectedDeckPresenter?.Dispose();

            // Hủy đăng ký sự kiện khi GameBootstrapper bị destroy
            if (_deckSessionService != null)
            {
                _deckSessionService.OnDeckChanged -= UpdateDeckDebugView;
            }
        }
    }
}
