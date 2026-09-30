using UnityEngine;
using BeastLinkBattle.Gameplay.Battle.Models;

namespace BeastLinkBattle.Gameplay.Battle.Effects
{
    public class StunEffect : StatusEffect
    {
        public StunEffect(float duration) : base(duration) { }

        public override void OnApply(BaseEntityModel target)
        {
            target.IsStunned = true;
        }

        public override void OnRemove(BaseEntityModel target)
        {
            target.IsStunned = false;
        }
    }
}