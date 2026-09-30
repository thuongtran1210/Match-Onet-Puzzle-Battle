namespace BeastLinkBattle.Gameplay.Battle.Entities
{
    public enum Faction
    {
        Player, // Phe ta (di sang phải)
        Enemy   // Phe địch (di sang trái)
    }

    public enum EntityState
    {
        Idle,       // Ðứng yên, chưa làm gì
        Moving,
        Attacking,
        Retreating, // Rút lui (di chuyển về phía sau sau khi tấn công)
        Dead
    }

    public interface IDamageable
    {
        Faction EntityFaction { get; }
        bool IsAlive { get; }
        void TakeDamage(int amount);
        void Heal(int amount);
    }
}