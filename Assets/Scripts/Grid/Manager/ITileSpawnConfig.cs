namespace BeastLinkBattle.Grid.Manager
{
    public interface ITileSpawnConfig
    {
        float BeastWeight { get; }
        float EnergyWeight { get; }
        WeightedBeast[] Beasts { get; }
        WeightedEnergy[] Energies { get; }
    }
}