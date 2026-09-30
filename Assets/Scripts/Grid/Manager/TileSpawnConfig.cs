using UnityEngine;
using BeastLinkBattle.Gameplay.Data; // Ð?ng quên namespace ch?a BeastDefinition

namespace BeastLinkBattle.Grid.Manager
{
    // ScriptableObject này chứa cấu hình về cách spawn tile,
    // bao gồm trọng số cho từng loại tile và danh sách các loại Beast và Energy có thể spawn cùng với trọng số của chúng.
    [CreateAssetMenu(menuName = "Game/Tile Spawn Config")]
    public class TileSpawnConfig : ScriptableObject, ITileSpawnConfig
    {
        [Header("Type Weight")]
        [SerializeField] private float beastWeight = 0.7f;
        [SerializeField] private float energyWeight = 0.3f;

        [Header("Beasts")]
        [SerializeField] private WeightedBeast[] beasts;

        [Header("Energies")]
        [SerializeField] private WeightedEnergy[] energies;

        public float BeastWeight => beastWeight;
        public float EnergyWeight => energyWeight;
        public WeightedBeast[] Beasts => beasts;
        public WeightedEnergy[] Energies => energies;
    }

    // Các lớp WeightedBeast và WeightedEnergy này đại diện cho một loại Beast hoặc Energy cụ thể cùng với trọng số của nó, 
    // cho phép TileFactory quyết định loại nào sẽ spawn dựa trên xác suất đã định nghĩa trong config
    [System.Serializable]
    public class WeightedBeast : IWeighted
    {
        // Ð?I T? BeastType SANG BeastDefinition
        public BeastDefinition definition;
        public float weight;
        public float Weight => weight;
    }
    
    // Tương tự, WeightedEnergy đại diện cho một loại Energy cụ thể cùng với trọng số của nó.
    [System.Serializable]
    public class WeightedEnergy : IWeighted
    {
        public EnergyDefinition definition;
        public float weight;
        public float Weight => weight;
    }
}