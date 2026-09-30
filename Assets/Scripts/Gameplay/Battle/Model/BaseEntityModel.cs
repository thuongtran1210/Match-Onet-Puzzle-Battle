using UnityEngine;
using System;
using BeastLinkBattle.Gameplay.Battle.Entities;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.Battle.Behaviors;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Battle.Effects;

namespace BeastLinkBattle.Gameplay.Battle.Models
{
    public abstract class BaseEntityModel : IDamageable
    {
        public abstract Faction EntityFaction { get; }
        public bool IsAlive => CurrentHp > 0;
        public EntityState CurrentState { get; protected set; }
        public Vector3 Position { get; set; }

        protected int MaxHp;
        protected int CurrentHp;
        public int Damage { get; protected set; }

        public float MoveSpeed { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackCooldown { get; protected set; }
        public float LastAttackTime { get; set; }

        public ElementType Element { get; protected set; }
        public RoleType Role { get; protected set; }

        public BaseEntityModel CurrentTarget { get; set; }
        public AttackBehavior Behavior { get; protected set; }

        // Hu?ng di chuy?n s? do l?p con quy?t d?nh
        public abstract Vector3 MoveDirection { get; }

        public event Action OnAttack;
        public event Action<int> OnTakeDamage;
        public event Action OnDie;
        public event Action<int> OnHeal;
        public Vector3 StartPosition { get; protected set; }
        public bool IsStunned { get; set; } = false;
        public int MaxHP => MaxHp;
        public int CurrentHP => CurrentHp;
        public BaseEntityModel(int hp, int damage, float speed, float range, float cooldown, Vector3 startPos, ElementType element, RoleType role, AttackBehavior behavior)
        {
            MaxHp = hp; CurrentHp = hp; Damage = damage;
            MoveSpeed = speed; AttackRange = range; AttackCooldown = cooldown;
            Position = startPos;

            StartPosition = startPos; // Luu l?i v? trí g?c

            Element = element; Role = role; Behavior = behavior;

            ChangeState(EntityState.Idle); // Lúc sinh ra thì d?ng yên ch? d?i
        }

        public void ChangeState(EntityState newState)
        {
            CurrentState = newState;
        }

        public virtual void DealDamage(BaseEntityModel target, List<BaseEntityModel> allDefenders)
        {
            if (target != null && target.IsAlive && Behavior != null)
            {
                // Giao phó hoàn toàn vi?c dánh d?m cho Behavior x? lý
                Behavior.ExecuteAttack(this, target, allDefenders);
                OnAttack?.Invoke();
            }
        }
        protected List<StatusEffect> _activeEffects = new List<StatusEffect>();

        public void AddEffect(StatusEffect effect)
        {
            _activeEffects.Add(effect);
            effect.OnApply(this);
        }
        public void TickEffects(float deltaTime)
        {
            // Duy?t ngu?c list d? an toàn khi xóa ph?n t?
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                var effect = _activeEffects[i];
                effect.OnTick(this, deltaTime);

                if (effect.IsFinished)
                {
                    effect.OnRemove(this);
                    _activeEffects.RemoveAt(i);
                }
            }
        }
        public virtual void TakeDamage(int amount)
        {
            if (!IsAlive) return;
            CurrentHp -= amount;
            OnTakeDamage?.Invoke(amount);

            if (CurrentHp <= 0)
            {
                ChangeState(EntityState.Dead);
                OnDie?.Invoke();
            }
        }
        public virtual void Heal(int amount)
        {
            if (!IsAlive) return;

            CurrentHp += amount;
            if (CurrentHp > MaxHp) CurrentHp = MaxHp;

            OnHeal?.Invoke(amount);
        }
    }
}