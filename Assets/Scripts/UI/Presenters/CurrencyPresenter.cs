// File: Scripts/UI/Presenters/CurrencyPresenter.cs (Cập nhật)
using System;
using BeastLinkBattle.Services;
using BeastLinkBattle.UI.Views;

namespace BeastLinkBattle.UI.Presenters
{
    public class CurrencyPresenter : IDisposable
    {
        private readonly CurrencyView _view;
        private readonly ICurrencyService _currencyService;

        public CurrencyPresenter(CurrencyView view, ICurrencyService currencyService)
        {
            _view = view;
            _currencyService = currencyService;

            if (_currencyService != null)
            {
                _currencyService.OnCurrencyChanged += HandleCurrencyChanged;
            }

            RefreshView();
        }

        private void HandleCurrencyChanged(CurrencyType type, int newAmount)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            if (_view == null || _currencyService == null) return;

            int currentGold = _currencyService.GetBalance(CurrencyType.Gold);
            int currentDiamond = _currencyService.GetBalance(CurrencyType.Diamond);

            _view.UpdateUI(currentGold, currentDiamond);
        }


        public void Dispose()
        {
            if (_currencyService != null)
            {
                _currencyService.OnCurrencyChanged -= HandleCurrencyChanged;
            }
        }
    }
}