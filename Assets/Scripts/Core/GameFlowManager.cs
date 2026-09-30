// Scripts/Gameplay/GameState/GameFlowManager.cs
using UnityEngine;

namespace BeastLinkBattle.Gameplay.GameState
{
    public class GameFlowManager : MonoBehaviour
    {
        private IGameState _currentState;

        public IGameState CurrentState => _currentState;

        public void ChangeState(IGameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        private void Update()
        {
            _currentState?.Update();
        }
    }
}