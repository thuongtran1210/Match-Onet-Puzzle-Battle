// Scripts/Gameplay/GameState/GameOverState.cs
using System;
using UnityEngine;

namespace BeastLinkBattle.Gameplay.GameState
{
    public class GameOverState : IGameState
    {
        private bool _isWin;
        private int _starCount;
        public event Action<bool, int> OnGameOverEntered;
        public event Action OnGameOverExited;

        public void SetResult(bool isWin, int stars)
        {
            _isWin = isWin;
            _starCount = stars;
        }

        public void Enter()
        {
            Time.timeScale = 0f;
            OnGameOverEntered?.Invoke(_isWin, _starCount);
        }

        public void Update() { }

        public void Exit()
        {
            Time.timeScale = 1f;
            OnGameOverExited?.Invoke();
        }
    }
}