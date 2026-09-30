using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

namespace BeastLinkBattle.UI.Views
{
    // View này sẽ hiển thị thông tin về wave hiện tại, và có hiệu ứng cảnh báo khi bắt đầu wave boss.
    public class WaveView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private GameObject _bossWarningPanel;
        [SerializeField] private Image _screenRedBorder;

        [Header("Settings")]
        [SerializeField] private float _warningDisplayTime = 2.5f;

        private Coroutine _warningCoroutine;

        private void Awake()
        {
            if (_bossWarningPanel != null) _bossWarningPanel.SetActive(false);
            if (_screenRedBorder != null) _screenRedBorder.gameObject.SetActive(false);
        }

        // Cập nhật text hiển thị thông tin về wave hiện tại.
        public void UpdateWaveText(string text)
        {
            if (_waveText != null)
            {
                _waveText.text = text;
            }
        }
        // Phát hiệu ứng cảnh báo boss khi bắt đầu wave boss.
        public void PlayBossWarningEffect()
        {
            if (_warningCoroutine != null) StopCoroutine(_warningCoroutine);
            _warningCoroutine = StartCoroutine(ShowBossWarningRoutine());
        }

        // Coroutine để hiển thị hiệu ứng cảnh báo boss.
        private IEnumerator ShowBossWarningRoutine()
        {
            if (_bossWarningPanel != null)
            {
                _bossWarningPanel.SetActive(true);
                _bossWarningPanel.transform.localScale = Vector3.one;
                _bossWarningPanel.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);

                if (_screenRedBorder != null)
                {
                    _screenRedBorder.gameObject.SetActive(true);
                    _screenRedBorder.color = new Color(1f, 0f, 0f, 0f);
                    _screenRedBorder.DOFade(0.4f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                }

                yield return new WaitForSeconds(_warningDisplayTime);

                _bossWarningPanel.transform.DOKill();
                _bossWarningPanel.SetActive(false);

                if (_screenRedBorder != null)
                {
                    _screenRedBorder.DOKill();
                    _screenRedBorder.DOFade(0f, 0.2f).OnComplete(() => _screenRedBorder.gameObject.SetActive(false));
                }
            }
        }
    }
}