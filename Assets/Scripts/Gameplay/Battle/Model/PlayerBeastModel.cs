using BeastLinkBattle.Gameplay.Battle.Entities;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.Battle.Behaviors;
using UnityEngine;
using BeastLinkBattle.Gameplay.Data.Skills;
using System;

namespace BeastLinkBattle.Gameplay.Battle.Models
{
    public class PlayerBeastModel : BaseEntityModel
    {
        public override Faction EntityFaction => Faction.Player;
        public override Vector3 MoveDirection => Vector3.right;

        public BeastDefinition Definition { get; private set; }
        public int StarLevel { get; private set; }
        public float SplashRadius { get; private set; }

        public int CurrentMana { get; private set; }
        public int MaxMana { get; private set; }
        public SkillDefinition AutoSkill { get; private set; }

        // Sự kiện phát ra khi Nộ đầy và Beast bắt đầu tung chiêu
        public event Action<SkillDefinition, PlayerBeastModel> OnAutoSkillReady;
        // Sự kiện phát ra để UI cập nhật thanh Nộ dưới chân Beast
        public event Action<int, int> OnManaChanged;

        public PlayerBeastModel(BeastDefinition type, int hp, int damage, float speed, float range, float cooldown, Vector3 startPos, int starLevel, float splashRadius)
                : base(hp, damage, speed, range, cooldown, startPos, type.element, type.role, type.attackBehavior) // C?p nh?t base(...)
        {
            Definition = type;
            StarLevel = starLevel;
            SplashRadius = splashRadius;
            MaxMana = type.maxMana;
            AutoSkill = type.autoCastSkill;
            CurrentMana = 0;
        }
        public void AddMana(int amount)
        {
            if (CurrentMana >= MaxMana || AutoSkill == null) return;

            CurrentMana += amount;

            // Báo cho UI (nếu có) update thanh Mana
            OnManaChanged?.Invoke(CurrentMana, MaxMana);

            if (CurrentMana >= MaxMana)
            {
                TriggerAutoSkill();
            }
        }
        private void TriggerAutoSkill()
        {
            CurrentMana = 0; // Reset lại Nộ
            OnManaChanged?.Invoke(CurrentMana, MaxMana);

            // Phát sự kiện tung chiêu. Lớp BattleSimulationService sẽ lắng nghe sự kiện này.
            OnAutoSkillReady?.Invoke(AutoSkill, this);
        }
    }
}