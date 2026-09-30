using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Grid.Service;
using System.Collections.Generic;
using UnityEngine;
namespace BeastLinkBattle.Grid.Manager
{
    public class GridManager : IGridManager
    {
        private ITileFactory tileFactory;
        private IGridService gridService;
        public GridManager(ITileFactory tileFactory, IGridService gridService)
        {
            this.tileFactory = tileFactory;
            this.gridService = gridService;
        }
        // Fill grid bằng cách lấy danh sách các vị trí trống, xáo trộn chúng, sau đó điền nội dung theo cặp vào từng vị trí. 
        // Nếu có số lượng vị trí trống là lẻ, vị trí cuối cùng sẽ bị khóa để đảm bảo tất cả các cặp đều có đối tác.

        public void FillGrid()
        {
            List<Vector2Int> emptyPos = gridService.GetEmptyPositions();

            ShuffleList(emptyPos);
            for (int i = 0; i < emptyPos.Count - 1; i += 2) 
            {
                Vector2Int pos1 = emptyPos[i];
                Vector2Int pos2 = emptyPos[i + 1];
                ITileContent[] pair = tileFactory.CreateRandomContentPair();
                gridService.GetTile(pos1).SetContent(pair[0]);
                gridService.GetTile(pos2).SetContent(pair[1]);
            }

            if (emptyPos.Count % 2 != 0)
            {
                Vector2Int lastPos = emptyPos[emptyPos.Count - 1];
                gridService.GetTile(lastPos).SetState(TileState.Blocked);
                Debug.LogWarning($"B?n d? có s? ô l?, dã khóa ô cu?i cùng t?i {lastPos} d? d?m b?o ch?n c?p!");
            }
        }
        private void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}
