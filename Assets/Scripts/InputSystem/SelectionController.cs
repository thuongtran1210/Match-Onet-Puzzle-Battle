using UnityEngine;
using System;
using BeastLinkBattle.Grid.Service; // G?i GridTransform t? code cu c?a b?n

namespace BeastLinkBattle.InputSystem
{
    // SelectionController là một lớp chịu trách nhiệm quản lý việc chọn ô trên lưới dựa trên đầu vào từ IInputProvider.
    // Nó lắng nghe sự kiện nhấn chuột và chuyển đổi vị trí thế giới
    // sang vị trí lưới, sau đó xử lý logic chọn ô và phát ra sự kiện khi có yêu cầu ghép đôi hoặc khi lựa chọn được xóa.
    public class SelectionController
    {
        private readonly IInputProvider _inputProvider;
        private readonly float _cellSize;
        private readonly Vector3 _boardOrigin;
        private readonly IGridService _gridService;

        // Tr?ng thái ch?n
        private Vector2Int? _firstSelectedPos = null;

        public event Action<Vector2Int, Vector2Int> OnMatchRequested;

        public event Action<Vector2Int> OnTileSelected;
        public event Action OnSelectionCleared;

        public SelectionController(IInputProvider inputProvider, IGridService gridService, float cellSize, Vector3 boardOrigin)
        {
            _inputProvider = inputProvider;
            _gridService = gridService;
            _cellSize = cellSize;
            _boardOrigin = boardOrigin;

            _inputProvider.OnPointerDown += HandlePointerDown;
        }

        private void HandlePointerDown(Vector3 worldPos)
        {
            Vector3 localPos = worldPos - _boardOrigin;

            Vector2Int gridPos = GridTransform.WorldToGrid(localPos, _cellSize);

            ProcessSelection(gridPos);
        }

        private void ProcessSelection(Vector2Int gridPos)
        {
            var tile = _gridService.GetTile(gridPos);
            if (tile == null || tile.Content == null || tile.State == TileState.Blocked)
            {
                return;
            }

            if (_firstSelectedPos == null)
            {
                _firstSelectedPos = gridPos;
                OnTileSelected?.Invoke(gridPos);
            }
            else
            {
                if (_firstSelectedPos.Value == gridPos)
                {
                    ClearSelection();
                    return;
                }

                OnMatchRequested?.Invoke(_firstSelectedPos.Value, gridPos);
                ClearSelection();
            }
        }

        public void ClearSelection()
        {
            _firstSelectedPos = null;
            OnSelectionCleared?.Invoke(); 
        }

        public void Dispose()
        {
            _inputProvider.OnPointerDown -= HandlePointerDown;
        }
    }
}