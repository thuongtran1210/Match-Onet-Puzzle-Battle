// Scripts/Gameplay/GameState/PauseState.cs
using System;
using UnityEngine;
using BeastLinkBattle.InputSystem;

namespace BeastLinkBattle.Gameplay.GameState
{
    public class PauseState : IGameState
    {
        private readonly IInputProvider _inputProvider;
        public event Action OnPauseEntered;
        public event Action OnPauseExited;

        public PauseState(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
        }

        public void Enter()
        {
            Time.timeScale = 0f;
            _inputProvider.EnableInput(false);
            OnPauseEntered?.Invoke(); 
        }

        public void Update() { }

        public void Exit()
        {
            Time.timeScale = 1f;
            _inputProvider.EnableInput(true);
            OnPauseExited?.Invoke(); 
        }
    }
}