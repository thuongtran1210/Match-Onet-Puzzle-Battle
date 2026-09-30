using BeastLinkBattle.Gameplay;
using UnityEngine;
using BeastLinkBattle.Gameplay.Battle;
using BeastLinkBattle.Gameplay.Battle.Entities;
using BeastLinkBattle.Gameplay.Battle.Models;


namespace BeastLinkBattle.UI.Views
{
    // BattlefieldView là một lớp chịu trách nhiệm quản lý việc hiển thị các thực thể trong trận đấu trên sân.
    // Nó lắng nghe sự kiện khi các thực thể của người chơi và kẻ thù được tạo ra,
    // và sau đó tạo các đối tượng GameObject tương ứng trong cảnh để hiển thị chúng.
    // Khi một thực thể được tạo ra, BattlefieldView sẽ lấy prefab tương ứng từ định
    // nghĩa của chúng và khởi tạo chúng tại vị trí được chỉ định, đồng thời gán chúng vào một container chung để dễ quản lý.
    public class BattlefieldView : MonoBehaviour
    {
        [Header("Spawn Areas")]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _enemySpawnPoint;
        [SerializeField] private Transform _beastContainer;

        public Vector3 PlayerSpawnPos => _playerSpawnPoint.position;
        public Vector3 EnemySpawnPos => _enemySpawnPoint.position;

        private IBattleService _battleService;

        public void Setup(IBattleService battleService, IEnemySpawnerService enemySpawner)
        {
            _battleService = battleService;
            _battleService.OnPlayerBeastCreated += HandlePlayerBeastCreated;
            _battleService.OnEnemyCreated += HandleEnemyCreated;
            enemySpawner.OnWaveWarning += HandleWaveWarning;
        }

        private void HandlePlayerBeastCreated(PlayerBeastModel model)
        {
            GameObject prefabToSpawn = model.Definition.battlePrefab;

            if (prefabToSpawn == null)
            {
                Debug.LogError($"[BattlefieldView] {model.Definition.beastId} thi?u Battle Prefab!");
                return;
            }

            GameObject newBeastObj = Instantiate(prefabToSpawn, model.Position, Quaternion.identity, _beastContainer);

            if (newBeastObj.TryGetComponent(out BeastEntity entityView))
            {
                entityView.Bind(model);
            }
        }

        private void HandleEnemyCreated(EnemyModel model)
        {
            GameObject prefabToSpawn = model.Definition.battlePrefab;
            if (prefabToSpawn == null)
            {
                Debug.LogError($"[BattlefieldView] {model.Definition.enemyId} thi?u Battle Prefab!");
                return;
            }

            GameObject newBeastObj = Instantiate(prefabToSpawn, model.Position, Quaternion.identity, _beastContainer);
            newBeastObj.layer = LayerMask.NameToLayer("Enemy");

            if (model.IsBoss) newBeastObj.transform.localScale *= 1.8f;

            if (newBeastObj.TryGetComponent(out BeastEntity entityView))
            {
                entityView.Bind(model);
            }
        }

        private void HandleWaveWarning()
        {
            Debug.Log(">>> WARNING: MINI-BOSS IS APPROACHING! <<<");
        }

        private void OnDestroy()
        {
            if (_battleService != null)
            {
                _battleService.OnPlayerBeastCreated -= HandlePlayerBeastCreated;
                _battleService.OnEnemyCreated -= HandleEnemyCreated;
            }
        }
    }
}