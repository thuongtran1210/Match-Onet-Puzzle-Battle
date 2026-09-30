using UnityEngine;
using TMPro;
using DG.Tweening;
using BeastLinkBattle.Core;

namespace BeastLinkBattle.UI.Views
{
    // View này sẽ được spawn từ Pool khi cần hiển thị damage, sau đó tự động thu nhỏ và trả về Pool sau khi hết thời gian hiển thị.
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMesh; 
        [SerializeField] private float _lifeTime = 0.8f;
        [SerializeField] private float _floatHeight = 1.5f;

        public void Setup(int damageAmount, Color color, bool isCritical = false)
        {
            if (_textMesh == null) _textMesh = GetComponent<TextMeshPro>();
            transform.DOKill();
            _textMesh.DOKill();

            transform.localScale = Vector3.one;
            _textMesh.alpha = 1f; 

            _textMesh.text = damageAmount.ToString();
            _textMesh.color = color;

            if (isCritical)
            {
                transform.localScale = Vector3.one * 1.5f;
                transform.DOShakePosition(_lifeTime / 2, 0.2f);
            }

            transform.DOMoveY(transform.position.y + _floatHeight, _lifeTime).SetEase(Ease.OutBounce);

            _textMesh.DOFade(0f, _lifeTime).SetEase(Ease.InExpo).OnComplete(() =>
            {
                PoolManager.Instance.Release(gameObject);
            });
        }

        private void OnDisable()
        {
            transform.DOKill();
            if (_textMesh != null) _textMesh.DOKill();
        }
    }
}