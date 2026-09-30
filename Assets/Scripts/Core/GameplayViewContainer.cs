// Scripts/Core/GameplayViewContainer.cs
using UnityEngine;
using BeastLinkBattle.UI.Views;
using BeastLinkBattle.Grid.View;
using BeastLinkBattle.Gameplay.Battle;

namespace BeastLinkBattle.Core
{
    [System.Serializable] 
    public class GameplayViewContainer
    {
        public EnergyView energyView;
        public ComboView comboView;
        public SelectionView selectionView;
        public BattlefieldView battlefieldView;
        public BattleQueueView battleQueueView;
        public WaveView waveView;
        public TeamHealthView teamHealthView;
      
        [Header("End Game Views")]
        public GameOverView GameOverView;
        public WaveClearView WaveClearView;
    }
}