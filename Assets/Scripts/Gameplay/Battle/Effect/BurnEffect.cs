using UnityEngine;
using BeastLinkBattle.Gameplay.Battle.Models;

namespace BeastLinkBattle.Gameplay.Battle.Effects
{
    public class BurnEffect : StatusEffect
    {
        private int _damagePerTick;
        private float _tickInterval;
        private float _tickTimer;

        public BurnEffect(float duration, int damagePerTick, float tickInterval = 1.0f) : base(duration)
        {
            _damagePerTick = damagePerTick;
            _tickInterval = tickInterval;
            _tickTimer = tickInterval; // Bắt đầu đếm từ lúc hiệu ứng được áp dụng
        }

        public override void OnTick(BaseEntityModel target, float deltaTime)
        {
            base.OnTick(target, deltaTime);

            _tickTimer -= deltaTime;
            if (_tickTimer <= 0 && target.IsAlive)
            {
                target.TakeDamage(_damagePerTick);
                _tickTimer = _tickInterval;
            }
        }
    }
}