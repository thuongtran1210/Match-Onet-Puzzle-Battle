using BeastLinkBattle.Gameplay.Data;
using System;

namespace BeastLinkBattle.Services
{
    public interface IDeckSaveSystem
    {
        /// <summary>
        /// Lưu đội hình hiện tại của người chơi.
        /// </summary>
        /// <param name="deck">Đội hình cần lưu</param>
        /// <param name="slotId">Dành cho việc mở rộng lưu nhiều đội hình sau này (Vd: "Deck_1", "Deck_2")</param>
        void SaveDeck(PlayerDeck deck, string slotId = "DefaultDeck");

        /// <summary>
        /// Tải lên đội hình đã lưu.
        /// </summary>
        /// <returns>Trả về PlayerDeck. Nếu chưa có dữ liệu, trả về null hoặc deck mặc định.</returns>
        PlayerDeck LoadDeck(string slotId = "DefaultDeck");

        /// <summary>
        /// Kiểm tra xem người chơi đã có đội hình lưu trước đó chưa.
        /// </summary>
        bool HasSavedDeck(string slotId = "DefaultDeck");
        event Action<PlayerDeck> OnDeckSaved;
    }
}