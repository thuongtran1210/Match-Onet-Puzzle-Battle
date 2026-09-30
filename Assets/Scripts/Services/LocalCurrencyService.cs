using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastLinkBattle.Services
{
    public class LocalCurrencyService : MonoBehaviour, ICurrencyService
    {
        public event Action<CurrencyType, int> OnCurrencyChanged;

        private Dictionary<CurrencyType, int> _balances = new Dictionary<CurrencyType, int>();

        public void Initialize()
        {
            // Tải dữ liệu từ PlayerPrefs, mặc định cho 1000 Vàng và 100 Kim cương để test
            _balances[CurrencyType.Gold] = PlayerPrefs.GetInt("Currency_Gold", 1000);
            _balances[CurrencyType.Diamond] = PlayerPrefs.GetInt("Currency_Diamond", 100);
        }

        public int GetBalance(CurrencyType type)
        {
            return _balances.ContainsKey(type) ? _balances[type] : 0;
        }

        public bool HasEnough(CurrencyType type, int amount)
        {
            return GetBalance(type) >= amount;
        }

        public void AddCurrency(CurrencyType type, int amount)
        {
            if (amount <= 0) return;

            if (!_balances.ContainsKey(type)) _balances[type] = 0;
            _balances[type] += amount;

            SaveCurrency();
            OnCurrencyChanged?.Invoke(type, _balances[type]);
        }

        public bool SpendCurrency(CurrencyType type, int amount)
        {
            if (amount <= 0 || !HasEnough(type, amount)) return false;

            _balances[type] -= amount;

            SaveCurrency();
            OnCurrencyChanged?.Invoke(type, _balances[type]);
            return true;
        }

        public void SaveCurrency()
        {
            if (_balances.ContainsKey(CurrencyType.Gold))
                PlayerPrefs.SetInt("Currency_Gold", _balances[CurrencyType.Gold]);
            if (_balances.ContainsKey(CurrencyType.Diamond))
                PlayerPrefs.SetInt("Currency_Diamond", _balances[CurrencyType.Diamond]);

            PlayerPrefs.Save();
        }
    }
}