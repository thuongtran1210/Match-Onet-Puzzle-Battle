using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BeastLinkBattle.Grid.Controller;
using BeastLinkBattle.Grid.Service; 

namespace BeastLinkBattle.Grid.View
{
    // MatchLineView là một MonoBehaviour sử dụng LineRenderer để vẽ đường nối giữa các ô được chọn trong quá trình tìm kiếm đường đi (pathfinding) trên lưới.
    // Nó lắng nghe sự kiện OnDrawMatchPath từ BoardController để biết khi nào cần vẽ đường nối,
    // và sau đó sử dụng GridTransform để chuyển đổi các tọa độ lưới thành tọa độ thế giới để hiển thị đường nối chính xác trên màn hình. 
    // Đường nối sẽ hiển thị trong một khoảng thời gian ngắn trước khi tự động biến mất.
    [RequireComponent(typeof(LineRenderer))]
    public class MatchLineView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BoardController _controller;

        [Header("Settings")]
        [SerializeField] private float _cellSize = 1.1f;
        [Tooltip("Th?i gian du?ng n?i hi?n th? trên màn hình tru?c khi bi?n m?t")]
        [SerializeField] private float _displayDuration = 0.3f;
        [Tooltip("Tr?c Z d? dua LineRenderer n?i lên trên Tile")]
        [SerializeField] private float zOffset = -0.5f;

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 0; 
            _lineRenderer.useWorldSpace = true;
        }

        private void OnEnable()
        {
            if (_controller != null)
                _controller.OnDrawMatchPath += DrawPath;
        }

        private void OnDisable()
        {
            if (_controller != null)
                _controller.OnDrawMatchPath -= DrawPath;
        }

        private void DrawPath(List<Vector2Int> gridPath)
        {
            if (gridPath == null || gridPath.Count == 0) return;

            StopAllCoroutines();
            StartCoroutine(DisplayPathRoutine(gridPath));
        }

        private IEnumerator DisplayPathRoutine(List<Vector2Int> gridPath)
        {
            _lineRenderer.positionCount = gridPath.Count;
            Vector3 boardOrigin = _controller.transform.position;

            for (int i = 0; i < gridPath.Count; i++)
            {
                Vector3 worldPos = GridTransform.GridToWorld(gridPath[i], _cellSize) + boardOrigin;
                worldPos.z += zOffset;

                _lineRenderer.SetPosition(i, worldPos);
            }
            yield return new WaitForSeconds(_displayDuration);
            _lineRenderer.positionCount = 0;
        }
    }
}