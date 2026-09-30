using BeastLinkBattle.UI;
using UnityEngine;

public class ComboTimeState : IGameState
{
    public void Enter()
    {
        Debug.Log("--- STATE: COMBO TIME  ---");
        UIManager.Instance.ShowPanel(PanelType.BattleQueueHUD);
    }

    public void Exit()
    {
        Debug.Log("--- EXIT COMBO TIME ---");
        UIManager.Instance.HidePanel(PanelType.BattleQueueHUD);
    }

    public void Update()
    {

    }
}
