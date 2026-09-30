using BeastLinkBattle.Gameplay.Data;

namespace BeastLinkBattle.Grid.Data
{
    public interface ITileContent
    {
        TileType Type { get; }
        bool IsMatchWith(ITileContent other);

    }
    public class BeastContent : ITileContent
    {
        public TileType Type => TileType.Beast;
        public BeastDefinition Definition { get; private set; } 

        public BeastContent(BeastDefinition def) => Definition = def;

        public bool IsMatchWith(ITileContent other)
        {
            if (other == null) return false;
            if (other is BeastContent otherBeast)
            {
                return this.Definition == otherBeast.Definition;
            }
            return false;
        }
    }

    public class EnergyContent : ITileContent
    {
        public TileType Type => TileType.Energy;
        public EnergyDefinition Definition { get; private set; }

        public EnergyContent(EnergyDefinition def) => Definition = def;

        public bool IsMatchWith(ITileContent other)
        {
            if (other == null) return false;
            if (other is EnergyContent otherEnergy)
            {
                // So sánh reference của ScriptableObject để đảm bảo tính nhất quán
                return this.Definition == otherEnergy.Definition;
            }
            return false;
        }
    }
}
