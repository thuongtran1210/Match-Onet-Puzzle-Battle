using UnityEngine;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.Battle.Behaviors; 

namespace BeastLinkBattle.Gameplay.Battle.Data
{
    // ScriptableObject định nghĩa thông tin chi tiết của một kẻ địch, bao gồm:
    // - Tên định danh, nguyên tố và vai trò của kẻ địch
    // - Hành vi tấn công của kẻ địch (tham chiếu đến một AttackBehavior)
    // - Thông tin hiển thị trên UI, bao gồm màu sắc và biểu tượng
    // - Prefab sử dụng cho trận đấu
    
    [CreateAssetMenu(menuName = "Game/Data/Enemy Definition", fileName = "Enemy_")]
    public class EnemyDefinition : ScriptableObject
    {
        [Header("Identity & Save Data")]
        public string enemyId; // Chuẩn hóa từ idName
        public ElementType element;
        public RoleType role;

        [Header("UI Presentation")]
        public string displayName;
        public Sprite uiIcon; // Bổ sung để hiển thị HUD mục tiêu trong trận

        [Header("Prefabs")]
        public GameObject battlePrefab;

        [Header("Base Stats (1 Star)")]
        public int baseHp = 100;
        public int baseDamage = 10;
        public float moveSpeed = 2.0f;
        public float attackRange = 1.2f;
        public float attackCooldown = 1.0f;

        [Header("Combat Logic")]
        public AttackBehavior attackBehavior;
    }
}