using UnityEngine;

namespace BeastLinkBattle.Gameplay.Data
{
    [CreateAssetMenu(menuName = "Game/Data/Upgrade Config", fileName = "UpgradeConfig")]
    public class UpgradeConfig : ScriptableObject
    {
        [Header("Số thẻ cần để nâng lên cấp tiếp theo")]
        [Tooltip("Index 0 = số thẻ để lên Lv 2, Index 1 = số thẻ để lên Lv 3...")]
        public int[] commonRequirements;
        public int[] rareRequirements;
        public int[] epicRequirements;
        public int[] legendaryRequirements;

        public int GetRequiredCards(ItemRarity rarity, int currentLevel)
        {
            int[] reqArray = rarity switch
            {
                ItemRarity.Rare => rareRequirements,
                ItemRarity.Epic => epicRequirements,
                ItemRarity.Legendary => legendaryRequirements,
                _ => commonRequirements
            };

            // Nếu đã đạt Max Level, trả về -1 (hoặc số lượng lớn tùy logic game)
            if (currentLevel - 1 >= reqArray.Length) return -1;

            return reqArray[currentLevel - 1];
        }
    }
}