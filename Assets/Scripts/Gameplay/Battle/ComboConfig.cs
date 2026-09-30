using UnityEngine;

namespace BeastLinkBattle.Gameplay
{
    [CreateAssetMenu(menuName = "Game/Gameplay/Combo Config")]
    public class ComboConfig : ScriptableObject
    {
        [Header("Time Settings")]
        [Tooltip("Th?i gian kh?i t?o khi match c?p d?u tiên (giây)")]
        public float initialTime = 5.0f;

        [Tooltip("Th?i gian thu?ng thêm m?i khi match thành công (giây)")]
        public float bonusTimePerMatch = 2.0f;

        [Tooltip("Gi?i h?n th?i gian t?i da c?a thanh Combo (giây)")]
        public float maxTimeLimit = 10.0f;


    }
}