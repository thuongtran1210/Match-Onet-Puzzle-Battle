using UnityEngine;
using System;
using BeastLinkBattle.Grid.Controller;
using BeastLinkBattle.Gameplay.Battle;
using BeastLinkBattle.InputSystem;
using BeastLinkBattle.Gameplay;

namespace BeastLinkBattle.Core.Coordinators
{
    public class GridFlowCoordinator : IDisposable
    {
        private readonly BoardController _boardController;
        private readonly SelectionController _selectionController;
        private readonly IEnergyService _energyService;
        private readonly IBattleService _battleService;
        private readonly IComboService _comboService;

        public GridFlowCoordinator(
            BoardController boardController,
            SelectionController selectionController,
            IEnergyService energyService,
            IBattleService battleService,
            IComboService comboService)
        {
            _boardController = boardController;
            _selectionController = selectionController;
            _energyService = energyService;
            _battleService = battleService;
            _comboService = comboService;
        }

        public void Initialize()
        {
            _selectionController.OnMatchRequested += HandleMatchRequested;
            _boardController.OnMatchCompleted += HandleMatchCompleted;
        }

        private void HandleMatchRequested(Vector2Int a, Vector2Int b)
        {
            _boardController.ProcessMatch(a, b);
        }

        private void HandleMatchCompleted(Grid.Data.ITileContent content, Vector2Int a, Vector2Int b)
        {
            _energyService.ProcessMatchedContent(content);
            _battleService.ProcessMatchedContent(content);
            _comboService.RegisterMatch();
        }

        public void Dispose()
        {
            if (_selectionController != null) _selectionController.OnMatchRequested -= HandleMatchRequested;
            if (_boardController != null) _boardController.OnMatchCompleted -= HandleMatchCompleted;
        }
    }
}