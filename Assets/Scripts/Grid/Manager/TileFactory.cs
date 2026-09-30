using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Gameplay.Data;
using UnityEngine;
using System.Collections.Generic;

namespace BeastLinkBattle.Grid.Manager
{
    public class TileFactory : ITileFactory
    {
        private PlayerDeck _playerDeck;
        private SpawnMode _currentMode = SpawnMode.Mixed;

        public TileFactory(PlayerDeck playerDeck)
        {
            _playerDeck = playerDeck;

            if (_playerDeck.SelectedBeasts == null || _playerDeck.SelectedBeasts.Count == 0)
            {
                Debug.LogError("[TileFactory] Deck của người chơi không có Beast nào! Cần xử lý fallback.");
            }
        }

        public void SetSpawnMode(SpawnMode mode)
        {
            _currentMode = mode;
        }

        public ITileContent CreateRandomContent()
        {
            bool isBeast = IsSpawningBeast();

            if (isBeast)
            {
                int randomIndex = Random.Range(0, _playerDeck.SelectedBeasts.Count);
                return new BeastContent(_playerDeck.SelectedBeasts[randomIndex]);
            }
            else
            {
                int randomIndex = Random.Range(0, _playerDeck.SelectedEnergies.Count);
                EnergyDefinition def = _playerDeck.SelectedEnergies[randomIndex];
                return new EnergyContent(def);
            }
        }

        public ITileContent[] CreateRandomContentPair()
        {
            bool isBeast = IsSpawningBeast();

            if (isBeast)
            {
                int randomIndex = Random.Range(0, _playerDeck.SelectedBeasts.Count);
                var def = _playerDeck.SelectedBeasts[randomIndex];
                return new ITileContent[] { new BeastContent(def), new BeastContent(def) };
            }
            else
            {
                int randomIndex = Random.Range(0, _playerDeck.SelectedEnergies.Count);
                var type = _playerDeck.SelectedEnergies[randomIndex];
                return new ITileContent[] { new EnergyContent(type), new EnergyContent(type) };
            }
        }
        private bool IsSpawningBeast()
        {
            if (_currentMode == SpawnMode.BeastOnly) return true;
            if (_currentMode == SpawnMode.EnergyOnly) return false;

            // Đếm số lượng bài mang vào
            int beastCount = _playerDeck.SelectedBeasts != null ? _playerDeck.SelectedBeasts.Count : 0;
            int energyCount = _playerDeck.SelectedEnergies != null ? _playerDeck.SelectedEnergies.Count : 0;
            int totalCards = beastCount + energyCount;

            // Tránh lỗi chia cho 0 nếu deck trống hoàn toàn
            if (totalCards == 0) return true;

            // Sinh 1 số ngẫu nhiên từ 0 đến (totalCards - 1)
            int randomRoll = Random.Range(0, totalCards);

            // Nếu số roll nằm trong khoảng số lượng Beast thì sinh Beast, ngược lại sinh Energy
            return randomRoll < beastCount;
        }
    }
}