using UnityEngine;
using BeastLinkBattle.InputSystem;
using BeastLinkBattle.Grid.Controller;
using BeastLinkBattle.Grid.Service;

namespace BeastLinkBattle.Grid.View
{

    // SelectionView là một MonoBehaviour chịu trách nhiệm hiển thị hiệu ứng chọn ô trên lưới khi người chơi tương tác với các ô.
    // Nó lắng nghe các sự kiện từ SelectionController để biết khi nào có ô được
    // chọn hoặc khi nào cần xóa hiệu ứng chọn, và cập nhật vị trí của hiệu ứng chọn (selection highlight) tương ứng
    // dựa trên tọa độ lưới được cung cấp. Nó sử dụng GridTransform để chuyển đổi giữa tọa độ lưới và tọa độ thế giới
    // để đảm bảo hiệu ứng chọn hiển thị chính xác trên màn hình.

    public class SelectionView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject selectionHighlightPrefab;
        [SerializeField] private float cellSize = 1.1f;
        [SerializeField] private float zOffset = -0.1f; 

        private SelectionController _selectionController;
        private BoardController _boardController;
        private GameObject _highlightInstance;
        public void Setup(SelectionController controller, BoardController boardController)
        {
            _selectionController = controller;
            _boardController = boardController;
            _selectionController.OnTileSelected += HandleTileSelected;
            _selectionController.OnSelectionCleared += HandleSelectionCleared;
            if (selectionHighlightPrefab != null)
            {
                _highlightInstance = Instantiate(selectionHighlightPrefab, transform);
                _highlightInstance.SetActive(false);
            }
        }

        private void HandleTileSelected(Vector2Int gridPos)
        {
            if (_highlightInstance == null) return;
            Vector3 worldPos = GridTransform.GridToWorld(gridPos, cellSize) + _boardController.transform.position;
            worldPos.z += zOffset;

            _highlightInstance.transform.position = worldPos;
            _highlightInstance.SetActive(true);
        }

        private void HandleSelectionCleared()
        {
            if (_highlightInstance != null)
            {
                _highlightInstance.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (_selectionController != null)
            {
                _selectionController.OnTileSelected -= HandleTileSelected;
                _selectionController.OnSelectionCleared -= HandleSelectionCleared;
            }
        }
    }
}