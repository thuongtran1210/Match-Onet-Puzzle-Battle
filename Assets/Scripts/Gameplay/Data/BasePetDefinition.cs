using UnityEngine;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Data.Skills; // Thêm dòng này

namespace BeastLinkBattle.Gameplay.Data
{
    // ScriptableObject định nghĩa thông tin cơ bản của một thú cưng, bao gồm:
    // - Tên và biểu tượng của thú cưng
    // - Các yếu tố nguyên tố mà thú cưng có thể sử dụng
    // - Các chỉ số buff thụ động mà thú cưng mang lại
    // - Kỹ năng chủ động của thú cưng (tham chiếu đến một SkillDefinition)

    [CreateAssetMenu(menuName = "Game/Data/Base Pet Definition", fileName = "BasePet_")]
    public class BasePetDefinition : ScriptableObject
    {
        [Header("Identity & Save Data")]
        [Tooltip("ID dùng để lưu trữ vào Save/Load system (vd: pet_dragon_01)")]
        public string petId;

        [Header("UI Presentation")]
        public string displayName; 
        public Sprite uiIcon;     

        [Header("Spawn Filter")]
        public List<ElementType> allowedElements;

        [Header("Passive Buffs")]
        public float bonusDamageMultiplier = 1.2f;
        public float bonusHpMultiplier = 1.1f;

        [Header("Active Skill")]
        [Tooltip("Kéo thả kỹ năng vào đây nè ní")]
        public SkillDefinition activeSkill;
    }
}