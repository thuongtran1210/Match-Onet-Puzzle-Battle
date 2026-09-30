using System.Collections.Generic;
using BeastLinkBattle.Grid.Data;
using UnityEngine;

namespace BeastLinkBattle.Grid.Service
{
    // RespawnRequest là một lớp đơn giản để chứa thông tin về các vị trí cần respawn và nội dung cụ thể (nếu có) 
    // để respawn vào các vị trí đó. Nó có thể được sử dụng để truyền thông tin từ hệ thống quản lý respawn 
    // đến hệ thống lưới để thực hiện việc respawn các ô.
    public class RespawnRequest
    {
        public List<Vector2Int> Positions { get; }

        public ITileContent SpecificContent { get; }

        public RespawnRequest(List<Vector2Int> positions, ITileContent specificContent = null)
        {
            Positions = positions;
            SpecificContent = specificContent;
        }
    }
}