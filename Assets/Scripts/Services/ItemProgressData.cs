// Scripts/Services/ItemProgressData.cs
namespace BeastLinkBattle.Services
{
    [System.Serializable]
    public class ItemProgressData
    {
        public string itemId;
        public int level = 1;
        public int cardCount = 0; // Số mảnh/thẻ đang sở hữu
    }
}