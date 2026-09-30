using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeastLinkBattle.UI.Views
{

    // View này sẽ hiển thị sau khi người chơi hoàn thành một wave, 
    // để thông báo về việc hoàn thành wave và số lượng thú cưng còn sống sót. 
    // Nó cũng sẽ có một nút để người chơi tiếp tục sang wave tiếp theo.
    public class WaveClearView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _nextWaveButton;
        [SerializeField] private TextMeshProUGUI _waveInfoText;
        [SerializeField] private TextMeshProUGUI _survivorText;

        public event Action OnNextWaveClicked;

        private void Awake()
        {
            if (_nextWaveButton != null)
            {
                _nextWaveButton.onClick.AddListener(() => OnNextWaveClicked?.Invoke());
            }
        }

        public void DisplayWaveClearInfo(int waveIndex, int survivedCount)
        {
            if (_waveInfoText != null)
            {
                _waveInfoText.text = $"HOAN THANH WAVE {waveIndex}!";
            }

            if (_survivorText != null)
            {
                _survivorText.text = $"Quai vat song sot: {survivedCount}";
            }
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
