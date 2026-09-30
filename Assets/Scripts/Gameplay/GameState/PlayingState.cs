using BeastLinkBattle.Grid.Manager;
using BeastLinkBattle.InputSystem;
using BeastLinkBattle.UI;
using UnityEngine;

namespace BeastLinkBattle.Gameplay.GameState
{
    public class PlayingState : IGameState
    {
        private readonly IInputProvider _inputProvider;
        private readonly ITileFactory _tileFactory;

        public PlayingState(IInputProvider inputProvider, ITileFactory tileFactory)
        {
            _inputProvider = inputProvider;
            _tileFactory = tileFactory;
        }

        public void Enter()
        {
            _inputProvider.EnableInput(true);
            _tileFactory.SetSpawnMode(SpawnMode.BeastOnly);
            UIManager.Instance.ShowPanel(PanelType.MainHUD);
            UIManager.Instance.HidePanel(PanelType.BattleQueueHUD);
        }

        public void Exit() { }
        public void Update() { }
    }
}