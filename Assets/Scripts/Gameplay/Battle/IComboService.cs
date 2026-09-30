using System;

namespace BeastLinkBattle.Gameplay
{
    // Interface cho ComboService, nơi sẽ xử lý logic combo của trận đấu, bao gồm:
    // - Quản lý số combo hiện tại và thời gian còn lại của combo
    // - Cập nhật combo khi có một match thành công
    // - Kết thúc combo khi thời gian hết hoặc khi bị ép kết thúc
    
    public interface IComboService
    {
        event Action<int> OnComboUpdated; 
        event Action<float> OnTimerUpdated; 
        event Action OnComboEnded; 

        bool IsActive { get; }
        int CurrentCombo { get; }
        float TimeLeft { get; }
        float MaxTimeLimit { get; }

        void RegisterMatch(); 
        void Tick(float deltaTime); 
        void ForceEndCombo(); 
    }
}