using System;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Services;
using BeastLinkBattle.UI.Views;
using UnityEngine;

namespace BeastLinkBattle.UI.Presenters
{
    public class SelectedDeckPresenter : IDisposable
    {
        private readonly SelectedDeckView _view;
        private readonly IDeckSaveSystem _deckSaveSystem;

        public SelectedDeckPresenter(SelectedDeckView view, IDeckSaveSystem deckSaveSystem)
        {
            _view = view;
            _deckSaveSystem = deckSaveSystem;
        }

        public void Initialize()
        {
            LoadAndDisplayDeck();

            // Đăng ký lắng nghe sự thay đổi
            _deckSaveSystem.OnDeckSaved += HandleDeckSaved;
        }
        private void HandleDeckSaved(PlayerDeck updatedDeck)
        {
            // Bất cứ khi nào PetRosterPresenter gọi SaveCurrentDeck(), hàm này sẽ tự chạy
            _view.DisplayDeck(updatedDeck);
        }

        private void LoadAndDisplayDeck()
        {
            PlayerDeck currentDeck = _deckSaveSystem.LoadDeck("DefaultDeck");
            if (currentDeck != null)
            {
                _view.DisplayDeck(currentDeck);
            }
        }

        public void Dispose()
        {
            _deckSaveSystem.OnDeckSaved -= HandleDeckSaved;
        }
    }
}