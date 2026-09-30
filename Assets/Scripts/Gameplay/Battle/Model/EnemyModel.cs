using BeastLinkBattle.Gameplay.Battle.Data;
using BeastLinkBattle.Gameplay.Battle.Entities;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.Battle.Behaviors; // Namespace ch?a AttackBehavior
using UnityEngine;

namespace BeastLinkBattle.Gameplay.Battle.Models
{
    public class EnemyModel : BaseEntityModel
    {
        public override Faction EntityFaction => Faction.Enemy;
        public override Vector3 MoveDirection => Vector3.left;

        public EnemyDefinition Definition { get; private set; }
        public bool IsBoss { get; private set; }
        public int BountyReward { get; private set; }

        public EnemyModel(EnemyDefinition def, int hp, int damage, float speed, float range, float cooldown, Vector3 startPos, bool isBoss)
            : base(hp, damage, speed, range, cooldown, startPos, def.element, def.role, def.attackBehavior)
        {
            Definition = def;
            IsBoss = isBoss;
            BountyReward = isBoss ? 50 : 10;
        }

        public override void TakeDamage(int amount)
        {
            // Logic gi?m sát thuong cho Boss có th? gi? nguyên ho?c tùy bi?n thêm d?a trên Role
            int finalDamage = IsBoss ? amount / 2 : amount;
            base.TakeDamage(finalDamage);
        }
    }
}