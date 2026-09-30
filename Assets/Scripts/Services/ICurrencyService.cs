using System;

namespace BeastLinkBattle.Services
{
    public enum CurrencyType
    {
        Gold,
        Diamond
    }

    public interface ICurrencyService
    {
        // Sự kiện bắn ra khi số tiền thay đổi để UI cập nhật
        event Action<CurrencyType, int> OnCurrencyChanged;

        void Initialize();

        // Các hàm kiểm tra và thao tác cơ bản
        int GetBalance(CurrencyType type);
        bool HasEnough(CurrencyType type, int amount);
        void AddCurrency(CurrencyType type, int amount);
        bool SpendCurrency(CurrencyType type, int amount);

        // Hàm lưu dữ liệu (tương tự IInventoryService)
        void SaveCurrency();
    }
}