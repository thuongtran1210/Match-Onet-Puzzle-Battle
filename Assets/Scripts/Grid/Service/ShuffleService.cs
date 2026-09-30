using BeastLinkBattle.Grid.Data;
using System.Collections.Generic;
using UnityEngine;

namespace BeastLinkBattle.Grid.Service
{
    public class ShuffleService : IShuffleService
    {
        private readonly IGridService _gridService;
        private readonly IMatchService _matchService;

        public ShuffleService(IGridService gridService, IMatchService matchService)
        {
            _gridService = gridService;
            _matchService = matchService;
        }


        // ExecuteShuffle sẽ lấy tất cả các vị trí có nội dung trên lưới, sau đó thu thập nội dung của các ô đó vào một danh sách.
        // Sau đó, nó sẽ thực hiện việc xáo trộn nội dung trong danh sách và cập nhật lại nội dung cho các ô trên lưới theo thứ tự đã xáo trộn.
        // Sau mỗi lần xáo trộn, nó sẽ kiểm tra xem có bất kỳ cặp ô nào có thể ghép đôi được hay không bằng cách sử dụng MatchService. 
        // Nếu tìm thấy một bàn chơi hợp lệ, nó sẽ dừng lại và trả về danh sách các vị trí đã được xáo trộn. 
        // Nếu sau một số lần xáo trộn mà vẫn không tìm được bàn chơi hợp lệ, 
        // nó sẽ có thể giữ nguyên trạng thái hiện tại của lưới hoặc thông báo lỗi tùy theo thiết kế của trò chơi.
        public List<Vector2Int> ExecuteShuffle()
        {
            var positions = _gridService.GetOccupiedPositions();
            if (positions.Count < 2) return positions; // Không d? ô d? xáo

            List<ITileContent> contents = new List<ITileContent>();
            foreach (var pos in positions)
            {
                contents.Add(_gridService.GetTile(pos).Content);
            }

            int maxAttempts = 15; 
            int attempt = 0;
            bool validBoardFound = false;

            while (attempt < maxAttempts)
            {
                ShuffleUtility(contents);
                for (int i = 0; i < positions.Count; i++)
                {
                    _gridService.SetTileContent(positions[i], contents[i]);
                }
                if (_matchService.HasAnyMatch())
                {
                    validBoardFound = true;
                    break;
                }
                attempt++;
            }

            if (!validBoardFound)
            {
                // Xử lý trường hợp không tìm được bàn chơi hợp lệ sau nhiều lần xáo,
                //  có thể là thông báo lỗi hoặc giữ nguyên trạng thái hiện tại.
                Debug.LogWarning($"[ShuffleService] Không tìm được bàn chơi hợp lệ sau {maxAttempts} lần xáo. Giữ nguyên trạng thái hiện tại.");
            }

            return positions; 
        }

        private void ShuffleUtility<T>(List<T> list)
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