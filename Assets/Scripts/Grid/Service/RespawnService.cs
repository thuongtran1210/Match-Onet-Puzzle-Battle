using System.Collections.Generic;
using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Grid.Manager;

namespace BeastLinkBattle.Grid.Service
{

    // RespawnService là một lớp triển khai của IRespawnService,
    // chịu trách nhiệm thực hiện việc respawn các ô trên lưới dựa trên thông tin được cung cấp trong RespawnRequest.
    // Nó sử dụng IGridService để cập nhật nội dung của các ô trên lưới, và ITileFactory để tạo ra nội dung mới nếu cần thiết.
    public class RespawnService : IRespawnService
    {
        private readonly IGridService _gridService;
        private readonly ITileFactory _tileFactory;

        public RespawnService(IGridService gridService, ITileFactory tileFactory)
        {
            _gridService = gridService;
            _tileFactory = tileFactory;
        }

        public void ExecuteRespawn(RespawnRequest request)
        {
            foreach (var pos in request.Positions)
            {
                ITileContent contentToSpawn = request.SpecificContent;

                if (contentToSpawn == null)
                {
                    contentToSpawn = _tileFactory.CreateRandomContent();
                }

                _gridService.SetTileContent(pos, contentToSpawn);
            }
        }
    }
}