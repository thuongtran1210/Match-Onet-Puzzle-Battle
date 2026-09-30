namespace BeastLinkBattle.Gameplay.Data
{
    public enum RoleType
    {
        Tanker,    // HP cao, ưu tiên nhắm vào hàng đầu (Tanker/Warrior)
        Warrior,   // Sát thương vật lý ổn định, ưu tiên nhắm vào hàng đầu (Tanker/Warrior)
        Assassin,  // Sát thương vật lý cao, ưu tiên nhắm vào hàng sau (Assassin/Ranger)
        Mage,      // Sát thương phép diện rộng (AoE), ưu tiên nhắm vào hàng sau (Mage/Healer)
        Healer,    // Hỗ trợ hồi máu cho đồng đội, ưu tiên nhắm vào hàng sau (Mage/Healer)
        Ranger     // Sát thương vật lý tầm xa, ưu tiên nhắm vào hàng sau (Assassin/Ranger)
    }
}