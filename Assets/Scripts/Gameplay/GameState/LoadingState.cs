// Scripts/Gameplay/GameState/LoadingState.cs
using UnityEngine;
using BeastLinkBattle.Core;

namespace BeastLinkBattle.Gameplay.GameState
{
    public class LoadingState : IGameState
    {
        private readonly GameObject _loadingPanel;
        private readonly GameFlowManager _flowManager;
        private readonly IGameState _nextState; 

        public LoadingState(GameObject loadingPanel, GameFlowManager flowManager, IGameState nextState)
        {
            _loadingPanel = loadingPanel;
            _flowManager = flowManager;
            _nextState = nextState;
        }

        public void Enter()
        {
            Debug.Log("--- STATE: LOADING ---");
            if (_loadingPanel != null) _loadingPanel.SetActive(true);
            _flowManager.ChangeState(_nextState);
        }

        public void Update() { }

        public void Exit()
        {
            if (_loadingPanel != null) _loadingPanel.SetActive(false);
        }
    }
}