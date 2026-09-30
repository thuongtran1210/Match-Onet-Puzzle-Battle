using BeastLinkBattle.Gameplay;
using BeastLinkBattle.Grid.Data;
using UnityEngine;
using System;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Battle.Entities;
using BeastLinkBattle.Gameplay.Battle;
using BeastLinkBattle.Gameplay.Battle.Models;
using BeastLinkBattle.Gameplay.Battle.Data;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.Data.Skills;

namespace BeastLinkBattle.Gameplay
{
    public class BattleManager : IBattleService
    {
        public event Action<PlayerBeastModel> OnPlayerBeastCreated;
        public event Action<EnemyModel> OnEnemyCreated;

        private Dictionary<BeastDefinition, int> _matchQueue = new Dictionary<BeastDefinition, int>();
        private BattleSimulationService _simulation;
        private EntityFactory _entityFactory;
        public event Action<Dictionary<BeastDefinition, int>> OnQueueUpdated;
        // T?a d? g?c
        private Vector3 _playerSpawnPos;
        private Vector3 _enemySpawnPos;

        private BeastBattleConfig _config;
        private BasePetDefinition _currentPet;

        public BattleManager(BattleSimulationService sim, EntityFactory factory, IEnemySpawnerService enemySpawner, Vector3 playerSpawnPos, Vector3 enemySpawnPos, BeastBattleConfig config, BasePetDefinition currentPet)
        {
            _simulation = sim;
            _entityFactory = factory;
            _playerSpawnPos = playerSpawnPos;
            _enemySpawnPos = enemySpawnPos;
            _config = config;
            _currentPet = currentPet;

            enemySpawner.OnEnemySpawnRequested += SpawnEnemyBeast;
        }
        public void InitializeBasePets()
        {
            
        }

        private Vector3 GetFormationPosition(Vector3 basePos, RoleType role, Faction faction)
        {
            float xOffset = _config.GetRoleOffset(role);

            // Nếu là địch thì đảo ngược khoảng cách để xuất hiện ở phía bên kia của chiến trường
            if (faction == Faction.Enemy)
            {
                xOffset = -xOffset;
            }

            // Random một chút yOffset để tránh việc các con thú chồng lên nhau hoàn toàn khi cùng hàng
            float yOffset = UnityEngine.Random.Range(-0.5f, 0.5f);

            return new Vector3(basePos.x + xOffset, basePos.y + yOffset, basePos.z);
        }

        public bool IsQueueEmpty()
        {
            foreach (var kvp in _matchQueue)
            {
                if (kvp.Value > 0) return false;
            }
            return true;
        }

        // Phương thức này có thể được gọi từ UI để kiểm tra xem có còn thú nào trong hàng đợi để triệu hồi hay không, từ đó quyết định việc hiển thị nút triệu hồi hay không
        public bool HasBeastInQueue(BeastDefinition def)
        {
            return _matchQueue.ContainsKey(def) && _matchQueue[def] > 0;
        }
        private void SpawnPlayerBeast(BeastDefinition type, int starLevel)
        {
     
            Vector3 spawnPos = GetFormationPosition(_playerSpawnPos, type.role, Faction.Player);

            PlayerBeastModel model = _entityFactory.CreatePlayerBeast(type, starLevel, spawnPos);
            if (model == null) return;

            _simulation.AddBeast(model);
            OnPlayerBeastCreated?.Invoke(model);
        }

        private void SpawnEnemyBeast(EnemyDefinition def, int starLevel, bool isBoss)
        {
            Vector3 spawnPos = GetFormationPosition(_enemySpawnPos, def.role, Faction.Enemy);

            EnemyModel model = _entityFactory.CreateEnemyBeast(def, starLevel, isBoss, spawnPos);
            if (model == null) return;

            _simulation.AddBeast(model);
            OnEnemyCreated?.Invoke(model);
        }
        public void ResolveBattlePhase()
        {
            foreach (var kvp in _matchQueue)
            {
                BeastDefinition type = kvp.Key;
                int totalMatches = kvp.Value;

                int star1Count = totalMatches / 2;
                int star2Count = star1Count / 3;
                star1Count = star1Count % 3;

                int star3Count = star2Count / 3;
                star2Count = star2Count % 3;

                // Triệu hồi thú theo thứ tự từ cao đến thấp
                for (int i = 0; i < star3Count; i++) SpawnPlayerBeast(type, 3);
                for (int i = 0; i < star2Count; i++) SpawnPlayerBeast(type, 2);
                for (int i = 0; i < star1Count; i++) SpawnPlayerBeast(type, 1);
            }
            _matchQueue.Clear();
            OnQueueUpdated?.Invoke(_matchQueue);
        }

        public void ProcessMatchedContent(ITileContent content)
        {
            if (content is BeastContent beast)
            {
                if (!_matchQueue.ContainsKey(beast.Definition))
                    _matchQueue[beast.Definition] = 0;

                _matchQueue[beast.Definition]++;
                OnQueueUpdated?.Invoke(_matchQueue);
            }
        }

        // Kích hoạt kỹ năng dựa trên loại năng lượng được sử dụng (có thể là kỹ năng của pet hoặc kỹ năng hệ thống chung)
        public void ExecuteSkill(SkillDefinition skill)
        {

            // Kiểm tra xem kỹ năng có tồn tại không trước khi thực thi
            if (skill != null)
            {
                skill.Execute(_simulation);
            }
            else
            {
                Debug.LogWarning("[BattleManager] Kỹ năng truyền vào bị null, không thể kích hoạt!");
            }
        }

        // Phương thức này có thể được gọi khi người chơi nhấn nút triệu hồi trên UI,
        // hoặc khi muốn giải quyết một con thú cụ thể trong hàng đợi (ví dụ để ưu tiên triệu hồi một con thú nào đó trước)
        public void ResolveSingleBeast(BeastDefinition type)
        {
            if (_matchQueue.ContainsKey(type) && _matchQueue[type] > 0)
            {
                int currentCount = _matchQueue[type];
                int starLevelToSpawn = 1;
                int cost = 1;

              
                if (currentCount >= 9)
                {
                    starLevelToSpawn = 3;
                    cost = 9;
                }
                else if (currentCount >= 3)
                {
                    starLevelToSpawn = 2;
                    cost = 3;
                }

           
                _matchQueue[type] -= cost;

             
                SpawnPlayerBeast(type, starLevelToSpawn);

           
                if (_matchQueue[type] <= 0)
                {
                    _matchQueue.Remove(type);
                }

        
                OnQueueUpdated?.Invoke(_matchQueue);
            }
        }
    }

}