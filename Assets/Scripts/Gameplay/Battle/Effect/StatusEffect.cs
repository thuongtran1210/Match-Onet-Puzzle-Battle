using UnityEngine;
using BeastLinkBattle.Gameplay.Battle.Models;

namespace BeastLinkBattle.Gameplay.Battle.Effects
{
    public abstract class StatusEffect
    {
        public float Duration { get; protected set; }
        public float TimeLeft { get; protected set; }
        public bool IsFinished => TimeLeft <= 0;

        public StatusEffect(float duration)
        {
            Duration = duration;
            TimeLeft = duration;
        }


        public virtual void OnApply(BaseEntityModel target) { }


        public virtual void OnTick(BaseEntityModel target, float deltaTime)
        {
            TimeLeft -= deltaTime;
        }

        public virtual void OnRemove(BaseEntityModel target) { }
    }
}