// File: Scripts/UI/Presenters/HeaderPresenter.cs
using System;
using BeastLinkBattle.Services;
using BeastLinkBattle.UI.Views;

namespace BeastLinkBattle.UI.Presenters
{
    public class HeaderPresenter : IDisposable
    {
        private readonly HeaderView _view;

        // Các Presenter con
        private readonly CurrencyPresenter _currencyPresenter;

        public HeaderPresenter(HeaderView view, ICurrencyService currencyService)
        {
            _view = view;

            // 1. Khởi tạo các sub-presenter
            if (_view.CurrencyView != null)
            {
                _currencyPresenter = new CurrencyPresenter(_view.CurrencyView, currencyService);
            }

            // 2. Tương lai: Khởi tạo PlayerProfilePresenter, v.v.
        }

        public void Dispose()
        {
            // Giải phóng bộ nhớ và huỷ đăng ký event cho tất cả các con
            _currencyPresenter?.Dispose();
        }
    }
}