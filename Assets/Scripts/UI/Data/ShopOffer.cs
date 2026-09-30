using System;
using UnityEngine;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Grid.Data; // Chứa EnergyType

namespace BeastLinkBattle.UI.Data
{
    public enum OfferType
    {
        Beast,
        Pet,
        Energy
    }

    [Serializable]
    public class ShopOffer
    {
        public string offerId;
        public OfferType offerType;

        [Header("Rewards (Gán dữ liệu tương ứng với Offer Type)")]
        public BeastDefinition beastReward;
        public BasePetDefinition petReward;
        public EnergyDefinition energyReward;

        [Header("Price")]
        public int price;

        [Header("UI Overrides (Dành cho Energy hoặc ghi đè tuỳ chỉnh)")]
        public string customName;
        public Sprite customIcon;

        // Hàm hỗ trợ lấy tên vật phẩm
        public string GetItemName()
        {
            if (!string.IsNullOrEmpty(customName)) return customName;

            return offerType switch
            {
                OfferType.Beast => beastReward != null ? beastReward.beastId : "Unknown Beast",
                OfferType.Pet => petReward != null ? petReward.petId : "Unknown Pet",
                OfferType.Energy => energyReward != null ? energyReward.displayName : "Unknown Energy",
                _ => "Unknown Item"
            };
        }

        public Sprite GetItemIcon()
        {
            if (customIcon != null) return customIcon;

            return offerType switch
            {
                OfferType.Beast => beastReward != null ? beastReward.uiIcon : null,
                OfferType.Pet => petReward != null ? petReward.uiIcon : null,
                // SỬA ĐỔI: Lấy Icon từ EnergyDefinition
                OfferType.Energy => energyReward != null ? energyReward.uiIcon : null,
                _ => null
            };
        }
    }
}