// Scripts/Gameplay/Battle/EntityFactory.cs
using BeastLinkBattle.Gameplay.Battle.Data;
using BeastLinkBattle.Gameplay.Battle.Models;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Grid.Data;
using UnityEngine;

namespace BeastLinkBattle.Gameplay.Battle
{
    public class EntityFactory
    {
        private readonly BeastBattleConfig _config;
        private readonly BasePetDefinition _currentPet;

        public EntityFactory(BeastBattleConfig config, BasePetDefinition currentPet = null)
        {
            _config = config;
            _currentPet = currentPet;
        }

        public PlayerBeastModel CreatePlayerBeast(BeastDefinition def, int starLevel, Vector3 spawnPos)
        {
            if (def == null) return null;

            // 1. Lấy hệ số nhân cơ bản dựa trên star level từ config
            float hpMulti = _config.GetHpMultiplier(starLevel);
            float dmgMulti = _config.GetDmgMultiplier(starLevel);
            float splashRadius = _config.GetSplashRadius(starLevel);

            // 2. Lấy hệ số nhân từ pet nếu có và áp dụng (chỉ áp dụng nếu pet hỗ trợ element của thú)
            float petHpBuff = 1f;
            float petDmgBuff = 1f;

            if (_currentPet != null && _currentPet.allowedElements.Contains(def.element))
            {
                petHpBuff = _currentPet.bonusHpMultiplier;
                petDmgBuff = _currentPet.bonusDamageMultiplier;
            }

            // 3. Nhân các hệ số với chỉ số cơ bản để tính HP và Damage cuối cùng
            int finalHp = Mathf.RoundToInt(def.baseHp * hpMulti * petHpBuff);
            int finalDmg = Mathf.RoundToInt(def.baseDamage * dmgMulti * petDmgBuff);

            return new PlayerBeastModel(
                def, finalHp, finalDmg,
                def.moveSpeed, def.attackRange, def.attackCooldown,
                spawnPos, starLevel, splashRadius
            );
        }

        public EnemyModel CreateEnemyBeast(EnemyDefinition def, int starLevel, bool isBoss, Vector3 spawnPos)
        {
            if (def == null) return null;

            float hpMulti = _config.GetHpMultiplier(starLevel);
            float dmgMulti = _config.GetDmgMultiplier(starLevel);

            float finalHpMulti = hpMulti * (isBoss ? _config.bossHpMultiplier : 1f);
            float finalDmgMulti = dmgMulti * (isBoss ? _config.bossDamageMultiplier : 1f);

            int finalHp = Mathf.RoundToInt(def.baseHp * finalHpMulti);
            int finalDmg = Mathf.RoundToInt(def.baseDamage * finalDmgMulti);

            return new EnemyModel(def, finalHp, finalDmg, def.moveSpeed, def.attackRange, def.attackCooldown, spawnPos, isBoss);
        }
    }
}