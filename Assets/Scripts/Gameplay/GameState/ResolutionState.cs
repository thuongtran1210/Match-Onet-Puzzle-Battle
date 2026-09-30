using BeastLinkBattle.Core;
using BeastLinkBattle.Gameplay.GameState;
using BeastLinkBattle.UI;
using UnityEngine;

public class ResolutionState : IGameState
{
    private readonly GameBootstrapper _bootstrapper;
    private readonly GameFlowManager _flowManager;
    private readonly AutoBattleState _autoBattleState;

    public ResolutionState(GameBootstrapper bootstrapper, GameFlowManager flowManager, AutoBattleState autoBattleState)
    {
        _bootstrapper = bootstrapper;
        _flowManager = flowManager;
        _autoBattleState = autoBattleState;
    }

    public void Enter()
    {
        _bootstrapper.SetBattleActive(false); 
        UIManager.Instance.ShowPanel(PanelType.BattleQueueHUD);
    }

    public void Exit() { }
    public void Update() { }
}