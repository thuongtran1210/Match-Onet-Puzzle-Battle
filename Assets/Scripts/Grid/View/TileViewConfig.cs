using UnityEngine;
using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Gameplay.Data; // Đảm bảo đã using namespace chứa EnergyDefinition

namespace BeastLinkBattle.Grid.View
{
    [CreateAssetMenu(menuName = "Game/View Config")]
    public class TileViewConfig : ScriptableObject
    {
        [Header("Base Grid Prefabs")]
        public GameObject emptyTilePrefab;
        public GameObject blockedTilePrefab;

        // Đã xóa bỏ mảng EnergyViewMapping ở đây vì không còn cần thiết nữa

        public GameObject GetContentPrefab(ITileContent content)
        {
            if (content is BeastContent beast)
            {
                return beast.Definition.gridPrefab;
            }

            if (content is EnergyContent energy)
            {
                // Truy xuất trực tiếp lưới prefab từ Definition, giống hệt như Beast
                return energy.Definition.gridPrefab;
            }

            return null;
        }
    }
}