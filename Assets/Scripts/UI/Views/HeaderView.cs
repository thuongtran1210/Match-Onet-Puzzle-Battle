// File: Scripts/UI/Views/HeaderView.cs
using UnityEngine;

namespace BeastLinkBattle.UI.Views
{
    public class HeaderView : MonoBehaviour
    {
        [Header("Sub-Views")]
        [SerializeField] private CurrencyView _currencyView;
        // Thêm các view khác trong tương lai ở đây:
        // [SerializeField] private PlayerProfileView _profileView;
        // [SerializeField] private SettingsButtonView _settingsBtnView;

        public CurrencyView CurrencyView => _currencyView;
    }
}