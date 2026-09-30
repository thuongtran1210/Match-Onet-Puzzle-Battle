namespace BeastLinkBattle.UI
{
    public interface IUIFlowService
    {
        void ShowPanel(PanelType type, bool hideOthers = false);
        void HidePanel(PanelType type);
    }
}
