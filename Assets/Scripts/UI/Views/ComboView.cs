using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BeastLinkBattle.UI.Views
{

    // View này chỉ chịu trách nhiệm hiển thị Combo Timer và hiệu ứng cảnh báo khi sắp hết thời gian combo.

    public class ComboView : MonoBehaviour
    {
        [Header("Audio & Polish")]
        [SerializeField] private AudioSource _tickAudioSource;

        [Header("UI References")]
        [SerializeField] private Image _timerBarFill;
        [SerializeField] private GameObject _comboContainer;

        private Vector3 _originalContainerPos;

        private void Awake()
        {
            if (_comboContainer != null)
            {
                _originalContainerPos = _comboContainer.transform.localPosition;
            }
        }


        public void ShowComboContainer()
        {
            if (_comboContainer != null && !_comboContainer.activeSelf)
                _comboContainer.SetActive(true);
        }

        public void HideComboContainer()
        {
            if (_comboContainer != null)
                _comboContainer.SetActive(false);
        }

        public void UpdateTimerBar(float fillRatio)
        {
            if (_timerBarFill != null)
            {
                _timerBarFill.fillAmount = Mathf.Clamp01(fillRatio);
            }
        }

        public void PlayWarningEffect()
        {
            if (DOTween.IsTweening(_timerBarFill) || DOTween.IsTweening(_comboContainer.transform)) return;

            _timerBarFill.DOColor(Color.red, 0.2f).SetLoops(-1, LoopType.Yoyo);

            RectTransform rect = _comboContainer.GetComponent<RectTransform>();
            if (rect != null) rect.DOShakeAnchorPos(1f, strength: 5f, vibrato: 10).SetLoops(-1);
            else _comboContainer.transform.DOShakePosition(1f, strength: 2f, vibrato: 10).SetLoops(-1);

            if (_tickAudioSource != null && !_tickAudioSource.isPlaying)
            {
                _tickAudioSource.Play();
            }
        }

        public void StopWarningEffect()
        {
            _timerBarFill.DOKill();
            if (_comboContainer != null) _comboContainer.transform.DOKill();

            _timerBarFill.color = Color.yellow;
            if (_comboContainer != null) _comboContainer.transform.localPosition = _originalContainerPos;

            if (_tickAudioSource != null) _tickAudioSource.Stop();
        }

        private void OnDestroy()
        {
            _timerBarFill.DOKill();
            if (_comboContainer != null) _comboContainer.transform.DOKill();
        }
    }
}