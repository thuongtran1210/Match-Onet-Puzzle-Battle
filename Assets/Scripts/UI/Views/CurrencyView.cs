using UnityEngine;
using TMPro;

namespace BeastLinkBattle.UI.Views
{
    public class CurrencyView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _goldText;
        [SerializeField] private TextMeshProUGUI _diamondText;
        public void UpdateUI(int goldAmount, int diamondAmount)
        {
            if (_goldText != null)
            {
                _goldText.text = goldAmount.ToString("N0");
            }

            if (_diamondText != null)
            {
                _diamondText.text = diamondAmount.ToString("N0");
            }
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}