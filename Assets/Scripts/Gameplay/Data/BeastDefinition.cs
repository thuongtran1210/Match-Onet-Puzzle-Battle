// Scripts/Gameplay/Data/BeastDefinition.cs
using UnityEngine;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.Battle.Behaviors;
using BeastLinkBattle.Gameplay.Data.Skills;

namespace BeastLinkBattle.Gameplay.Data
{
    // ScriptableObject định nghĩa thông tin chi tiết của một thú cưng, bao gồm:
    // - Tên định danh, nguyên tố và vai trò của thú cưng
    // - Hành vi tấn công của thú cưng (tham chiếu đến một AttackBehavior)
    // - Thông tin hiển thị trên UI, bao gồm màu sắc và biểu tượng
    // - Prefab sử dụng cho lưới và trận đấu
    
    [CreateAssetMenu(menuName = "Game/Data/Beast Definition", fileName = "Beast_")]
    public class BeastDefinition : ScriptableObject
    {
        [Header("Identity & Save Data")]
        [Tooltip("ID dùng để lưu trữ vào Save/Load system (vd: beast_fire_01)")]
        public string beastId; 
        public ElementType element;
        public RoleType role;

        [Header("UI Presentation")]
        public string displayName;
        public Sprite uiIcon;
        public Color uiColor = Color.white;

        [Header("Combat Logic")]
        public AttackBehavior attackBehavior;

        [Header("Prefabs")]
        public GameObject gridPrefab;
        public GameObject battlePrefab;

        [Header("Base Stats (1 Star)")]
        public int baseHp = 100;
        public int baseDamage = 10;
        public float moveSpeed = 2.0f;
        public float attackRange = 1.2f;
        public float attackCooldown = 1.0f;

        [Header("Auto Skill Logic")]
        [Tooltip("Kỹ năng quái thú sẽ tự động tung ra khi đầy Nộ")]
        public SkillDefinition autoCastSkill;

        [Tooltip("Lượng Nộ tối đa để có thể tung chiêu")]
        public int maxMana = 100;
    }
}