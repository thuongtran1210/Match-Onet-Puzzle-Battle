using UnityEngine;
using System.Collections.Generic;
using BeastLinkBattle.Grid.Controller;
using BeastLinkBattle.Grid.Service;
using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Core;

namespace BeastLinkBattle.Grid.View
{

    // BoardView là một MonoBehaviour chịu trách nhiệm hiển thị lưới và nội dung của các ô trên màn hình.
    // Nó lắng nghe các sự kiện từ BoardController để biết khi nào dữ liệu của lưới đã sẵn sàng và khi nào có sự cập nhật trên các ô,
    // từ đó cập nhật giao diện người dùng tương ứng.
    public class BoardView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BoardController _controller;
        [SerializeField] private TileViewConfig _viewConfig;
        [SerializeField] private float _cellSize = 1.1f;

        [Header("Spawn Transform")]
        [SerializeField] private Transform _boardContainer;

        private IGridService _gridService;
        private Dictionary<Vector2Int, GameObject> _spawnedContents = new Dictionary<Vector2Int, GameObject>();

        private void OnEnable()
        {
            _controller.OnBoardDataReady += HandleBoardReady;
            _controller.OnTileUpdated += UpdateSingleTile;
        }

        private void OnDisable()
        {
            _controller.OnBoardDataReady -= HandleBoardReady;
            _controller.OnTileUpdated -= UpdateSingleTile;
        }

        private void HandleBoardReady()
        {
            _gridService = _controller.GetGridService();
            DrawEntireBoard();
        }

        // DrawEntireBoard sẽ đi qua tất cả các ô trên lưới,
        // kiểm tra trạng thái của chúng và sinh ra các đối tượng GameObject tương ứng cho cả ô nền (base tile) và nội dung (content) nếu có.
        private void DrawEntireBoard()
        {
            foreach (var obj in _spawnedContents.Values)
            {
                PoolManager.Instance.Release(obj);
            }
            _spawnedContents.Clear();

            for (int x = 0; x < _gridService.Width; x++)
            {
                for (int y = 0; y < _gridService.Height; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    Tile tileData = _gridService.GetTile(pos);

                    if (tileData == null || tileData.State == TileState.Invalid) continue;

                    Vector3 worldPos = GridTransform.GridToWorld(pos, _cellSize) + _controller.transform.position;

                    SpawnBaseTile(tileData, worldPos);
                    SpawnContent(tileData, worldPos, pos);
                }
            }
        }

        // SpawnBaseTile sẽ sinh ra đối tượng GameObject đại diện cho ô nền (base tile) dựa trên trạng thái của ô đó (ví dụ: trống hoặc bị chặn).
        // SpawnContent sẽ sinh ra đối tượng GameObject đại diện cho nội dung của ô nếu có, dựa trên loại nội dung và sử dụng TileViewConfig để lấy prefab tương ứng.
        private void SpawnBaseTile(Tile tileData, Vector3 worldPos)
        {
            GameObject prefab = tileData.State == TileState.Blocked ? _viewConfig.blockedTilePrefab : _viewConfig.emptyTilePrefab;
            if (prefab != null)
            {
                PoolManager.Instance.Get(prefab, worldPos, Quaternion.identity, _boardContainer);
            }
        }

        // SpawnContent sẽ sinh ra đối tượng GameObject đại diện cho nội dung của ô nếu có,
        //  dựa trên loại nội dung và sử dụng TileViewConfig để lấy prefab tương ứng.   
        private void SpawnContent(Tile tileData, Vector3 worldPos, Vector2Int gridPos)
        {
            if (tileData.Content == null) return;
            GameObject contentPrefab = _viewConfig.GetContentPrefab(tileData.Content);
            if (contentPrefab != null)
            {
                GameObject instance = PoolManager.Instance.Get(contentPrefab, worldPos, Quaternion.identity, _boardContainer);
                _spawnedContents[gridPos] = instance;
            }
        }
 
        // UpdateSingleTile sẽ được gọi khi có sự kiện cập nhật từ BoardController, 
        // nó sẽ kiểm tra vị trí của ô được cập nhật và chỉ cần cập nhật lại đối tượng GameObject tương ứng cho ô đó thay vì phải vẽ lại toàn bộ bảng.
        private void UpdateSingleTile(Vector2Int pos)
        {
            if (_spawnedContents.TryGetValue(pos, out GameObject oldObject))
            {
                PoolManager.Instance.Release(oldObject);
                _spawnedContents.Remove(pos);
            }
            Tile tileData = _gridService.GetTile(pos);
            Vector3 worldPos = GridTransform.GridToWorld(pos, _cellSize) + _controller.transform.position;
            SpawnContent(tileData, worldPos, pos);
        }
    }
}