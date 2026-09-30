using BeastLinkBattle.Gameplay.Data;
using System;
/// <summary>
/// Interface định nghĩa các thao tác với Session Đội Hình.
/// Bất kỳ UI nào muốn thay đổi hoặc đọc đội hình đều phải thông qua đây.
/// </summary>
public interface IDeckSessionService
{
    PlayerDeck CurrentDeck { get; }
    int MaxBeasts { get; set; }
    int MaxEnergies { get; set; }

    event Action OnDeckChanged;

    void Initialize();
    void SelectPet(BasePetDefinition pet);

    /// <summary>
    /// Thêm hoặc bớt Beast. Trả về false nếu đạt giới hạn.
    /// </summary>
    bool ToggleBeast(BeastDefinition beast);

    /// <summary>
    /// Thêm hoặc bớt Energy. Trả về false nếu đạt giới hạn.
    /// </summary>
    bool ToggleEnergy(EnergyDefinition energy);
}