using System.Collections.Generic;
using UnityEngine;
using BeastLinkBattle.Gameplay.Battle.Models;
using BeastLinkBattle.Gameplay.Battle.Entities;
using System;
using BeastLinkBattle.Gameplay.Data;

namespace BeastLinkBattle.Gameplay.Battle
{
    public class BattleSimulationService
    {
      
        private List<BaseEntityModel> _playerBeasts = new List<BaseEntityModel>();
        private List<BaseEntityModel> _enemyBeasts = new List<BaseEntityModel>();

        public event Action OnEnemyDied; 
        public event Action OnPlayerBeastDied;

        public int ActivePlayerBeastsCount => _playerBeasts.Count;
        public List<BaseEntityModel> GetAllEnemies() => _enemyBeasts;
        public List<BaseEntityModel> GetAllPlayerBeasts() => _playerBeasts;
        public bool IsWaveActive { get; set; } = false;

        // --- CẤU HÌNH ---
        private const float AVOIDANCE_RADIUS = 0.5f; // Bán kính để bắt đầu áp dụng lực 
        private const float MAX_SEPARATION_FORCE = 0.7f; // Lực đẩy tối đa 
        private const int MAX_MELEE_ATTACKERS_PER_TARGET = 3; // Số lượng lính cận chiến tối đa tấn công cùng một mục tiêu 

        private Dictionary<BaseEntityModel, Vector3> _cachedSeparationForces = new Dictionary<BaseEntityModel, Vector3>();
        private float _playerSepTimer = 0f;
        private float _enemySepTimer = 0.05f; // Đặt lệch thời gian để tránh việc cả 2 phe cùng lúc tính
        private const float SEPARATION_UPDATE_RATE = 0.1f; //  Cứ 0.1 giây tính lại lực tách ra một lần, giúp giảm tải CPU so với việc tính mỗi frame
        private Dictionary<BaseEntityModel, int> _meleeTargetCounts = new Dictionary<BaseEntityModel, int>();
        private Dictionary<BaseEntityModel, float> _targetSearchTimers = new Dictionary<BaseEntityModel, float>();
        private const float TARGET_SEARCH_RATE = 0.25f; // 0.25 giây mới quét tìm mục tiêu tối ưu một lần
        public void AddBeast(BaseEntityModel beast)
        {
            if (beast.EntityFaction == Faction.Player) _playerBeasts.Add(beast);
            else _enemyBeasts.Add(beast);
        }
        private void ChangeTarget(BaseEntityModel beast, BaseEntityModel newTarget)
        {
            if (beast.CurrentTarget == newTarget) return;

            bool isMelee = (beast.Role == RoleType.Tanker || beast.Role == RoleType.Assassin);

            // Xóa đăng ký ở mục tiêu cũ
            if (isMelee && beast.CurrentTarget != null && _meleeTargetCounts.ContainsKey(beast.CurrentTarget))
            {
                _meleeTargetCounts[beast.CurrentTarget]--;
            }

            // Đăng ký ở mục tiêu mới
            if (isMelee && newTarget != null)
            {
                if (!_meleeTargetCounts.ContainsKey(newTarget))
                {
                    _meleeTargetCounts[newTarget] = 0;
                }
                _meleeTargetCounts[newTarget]++;
            }

            beast.CurrentTarget = newTarget;
        }
        public void Tick(float deltaTime)
        {
            _playerSepTimer += deltaTime;
            _enemySepTimer += deltaTime;

            bool updatePlayerSep = false;
            if (_playerSepTimer >= SEPARATION_UPDATE_RATE)
            {
                updatePlayerSep = true;
                _playerSepTimer = 0f;
            }

            bool updateEnemySep = false;
            if (_enemySepTimer >= SEPARATION_UPDATE_RATE)
            {
                updateEnemySep = true;
                _enemySepTimer = 0f;
            }

            UpdateTeam(_playerBeasts, _enemyBeasts, deltaTime, updatePlayerSep);
            UpdateTeam(_enemyBeasts, _playerBeasts, deltaTime, updateEnemySep);
        }

        private void UpdateTeam(List<BaseEntityModel> attackers, List<BaseEntityModel> defenders, float deltaTime, bool updateSeparation)
        {
            for (int i = attackers.Count - 1; i >= 0; i--)
            {
                var beast = attackers[i];
                if (!beast.IsAlive)
                {
                    // Xóa entity khỏi bộ nhớ cache khi chết
                    ChangeTarget(beast, null);
                    _targetSearchTimers.Remove(beast);

                    if (beast.EntityFaction == Faction.Enemy) OnEnemyDied?.Invoke();
                    else OnPlayerBeastDied?.Invoke();

                    attackers.RemoveAt(i);
                    continue;
                }

                beast.TickEffects(deltaTime);

                if (beast.IsStunned) continue;

   

                Vector3 separationForce = Vector3.zero;

                if (updateSeparation)
                {
                    foreach (var ally in attackers)
                    {
                        if (ally != beast && ally.IsAlive)
                        {
                            Vector3 diff = beast.Position - ally.Position;
                            float sqrMag = diff.sqrMagnitude;

                            if (sqrMag < AVOIDANCE_RADIUS * AVOIDANCE_RADIUS && sqrMag > 0.001f)
                            {
                                float distance = Mathf.Sqrt(sqrMag);
                                float pushStrength = 1f - (distance / AVOIDANCE_RADIUS);
                                separationForce += diff.normalized * pushStrength;
                            }
                        }
                    }
                    separationForce = Vector3.ClampMagnitude(separationForce, MAX_SEPARATION_FORCE);
                    _cachedSeparationForces[beast] = separationForce;
                }
                else
                {
                    _cachedSeparationForces.TryGetValue(beast, out separationForce);
                }

                if (beast.CurrentState == EntityState.Moving || beast.CurrentState == EntityState.Retreating || beast.CurrentState == EntityState.Idle)
                {
                    if (IsWaveActive)
                    {
                        beast.ChangeState(EntityState.Moving);

                        // CẬP NHẬT: Throttling - Chỉ tìm mục tiêu mới khi đã hết cooldown hoặc mục tiêu hiện tại đã chết
                        if (!_targetSearchTimers.ContainsKey(beast)) _targetSearchTimers[beast] = 0f;
                        _targetSearchTimers[beast] -= deltaTime;

                        if (beast.CurrentTarget == null || !beast.CurrentTarget.IsAlive || _targetSearchTimers[beast] <= 0f)
                        {
                            BaseEntityModel optimalTarget = FindOptimalTarget(beast, defenders); // Bỏ tham số allies
                            ChangeTarget(beast, optimalTarget);
                            _targetSearchTimers[beast] = TARGET_SEARCH_RATE; // Reset timer
                        }

                        if (beast.CurrentTarget != null)
                        {
                            Vector3 dirToTarget = beast.CurrentTarget.Position - beast.Position;
                            if (dirToTarget.sqrMagnitude <= beast.AttackRange * beast.AttackRange)
                            {
                                beast.ChangeState(EntityState.Attacking);
                            }
                            else
                            {
                                beast.Position += (dirToTarget.normalized + separationForce).normalized * (beast.MoveSpeed * deltaTime);
                            }
                        }
                        else
                        {
                            beast.Position += (beast.MoveDirection + separationForce).normalized * (beast.MoveSpeed * deltaTime);
                        }

                    }
                }
                else if (beast.CurrentState == EntityState.Attacking)
                {
                    if (beast.CurrentTarget == null || !beast.CurrentTarget.IsAlive)
                    {
                        ChangeTarget(beast, null); // Cập nhật Cache
                        beast.ChangeState(EntityState.Moving);
                        continue;
                    }

                    float sqrDist = (beast.Position - beast.CurrentTarget.Position).sqrMagnitude;
                    if (sqrDist > beast.AttackRange * beast.AttackRange)
                    {
                        beast.ChangeState(EntityState.Moving);
                        continue;
                    }
                    if (beast.Role == RoleType.Ranger)
                    {
                        float kitingDist = (beast.AttackRange * 0.5f);
                        if (sqrDist < kitingDist * kitingDist)
                        {
                            Vector3 retreatDir = (beast.Position - beast.CurrentTarget.Position).normalized;
                            beast.Position += (retreatDir + separationForce).normalized * (beast.MoveSpeed * 0.8f * deltaTime);
                        }
                    }
                    if (Time.time >= beast.LastAttackTime + beast.AttackCooldown)
                    {
                        beast.LastAttackTime = Time.time;
                        beast.DealDamage(beast.CurrentTarget, defenders);
                    }
                }
            }
        }


        private BaseEntityModel FindClosestTarget(BaseEntityModel attacker, List<BaseEntityModel> defenders)
        {
            float sqrAttackRange = attacker.AttackRange * attacker.AttackRange;

            BaseEntityModel closestTarget = null;
            float minSqrDist = float.MaxValue;

            foreach (var def in defenders)
            {
                if (!def.IsAlive) continue;

                float sqrDist = (attacker.Position - def.Position).sqrMagnitude;

                if (sqrDist <= sqrAttackRange && sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    closestTarget = def;
                }
            }
            return closestTarget;
        }


        private BaseEntityModel FindOptimalTarget(BaseEntityModel attacker, List<BaseEntityModel> defenders)
        {
            BaseEntityModel bestTarget = null;
            float minSqrDist = float.MaxValue;

            bool isAssassin = attacker.Role == RoleType.Assassin;
            bool isMeleeAttacker = attacker.Role == RoleType.Tanker || isAssassin;
            bool foundBacklineTarget = false;

            foreach (var def in defenders)
            {
                if (!def.IsAlive) continue;

                float sqrDist = (attacker.Position - def.Position).sqrMagnitude;
                if (isMeleeAttacker)
                {
                    // TỐI ƯU HÓA TẠI ĐÂY: Truy xuất O(1) từ Dictionary thay vì vòng lặp O(N)
                    _meleeTargetCounts.TryGetValue(def, out int currentMeleeCount);

                    // Nếu đã có mục tiêu và mục tiêu này không phải là mục tiêu hiện tại của mình
                    if (currentMeleeCount >= MAX_MELEE_ATTACKERS_PER_TARGET && attacker.CurrentTarget != def)
                    {
                        sqrDist += 1000f; // Vẫn ưu tiên penalty nếu đang có quá nhiều lính bu vào
                    }
                }

                if (isAssassin)
                {
                    bool isBacklineTarget = def.Role == RoleType.Mage || def.Role == RoleType.Ranger;

                    if (isBacklineTarget)
                    {
                        if (!foundBacklineTarget)
                        {
                            foundBacklineTarget = true;
                            minSqrDist = sqrDist;
                            bestTarget = def;
                        }
                        else if (sqrDist < minSqrDist)
                        {
                            minSqrDist = sqrDist;
                            bestTarget = def;
                        }
                    }
                    else if (!foundBacklineTarget && sqrDist < minSqrDist)
                    {
                        minSqrDist = sqrDist;
                        bestTarget = def;
                    }
                }
                else
                {
                    if (sqrDist < minSqrDist)
                    {
                        minSqrDist = sqrDist;
                        bestTarget = def;
                    }
                }
            }

            return bestTarget;
        }
        private int CountMeleeAttackers(BaseEntityModel target, List<BaseEntityModel> allies, BaseEntityModel currentAttacker)
        {
            int count = 0;
            foreach (var ally in allies)
            {
                if (ally == currentAttacker) continue; 

                if (ally.IsAlive && ally.CurrentTarget == target)
                {
                    if (ally.Role == RoleType.Tanker || ally.Role == RoleType.Assassin)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}