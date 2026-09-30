using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // B? sung d? load Scene
using DG.Tweening; // B? sung d? làm hi?u ?ng animation

namespace BeastLinkBattle.UI.Views
{
    [RequireComponent(typeof(CanvasGroup))] 
    public class GameOverHUD : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private GameObject[] stars; 
        [SerializeField] private Button retryButton;
        [SerializeField] private Button nextButton;

        [Header("Animation Container")]
        [Tooltip("Kéo object ch?a các UI bên trong vào dây d? làm hi?u ?ng phóng to")]
        [SerializeField] private RectTransform contentBox;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            if (retryButton != null)
                retryButton.onClick.AddListener(OnRetryClicked);

            if (nextButton != null)
                nextButton.onClick.AddListener(OnNextClicked);
        }

        public void Show(bool isWin, int starCount)
        {
            gameObject.SetActive(true);

            resultText.text = isWin ? "VICTORY" : "DEFEAT";
            resultText.color = isWin ? Color.green : Color.red;

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(i < starCount);
            }

            if (nextButton != null)
                nextButton.gameObject.SetActive(isWin);

            _canvasGroup.alpha = 0f;
            _canvasGroup.DOFade(1f, 0.3f).SetUpdate(true);

            if (contentBox != null)
            {
                contentBox.localScale = Vector3.zero;
                contentBox.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnRetryClicked()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnNextClicked()
        {
            Time.timeScale = 1f;


            Debug.Log("Next Level button clicked! Implement level progression logic here.");
        }

        private void OnDestroy()
        {
            _canvasGroup.DOKill();
            if (contentBox != null) contentBox.DOKill();
        }
    }
}