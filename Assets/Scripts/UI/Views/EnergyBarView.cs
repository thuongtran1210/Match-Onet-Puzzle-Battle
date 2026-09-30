using BeastLinkBattle.Grid.Data;
// Scripts/Grid/View/EnergyBarView.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using BeastLinkBattle.Gameplay.Data;

namespace BeastLinkBattle.UI.Views
{
    // View này sẽ được dùng để hiển thị thanh năng lượng của người chơi,
    // bao gồm cả hiệu ứng khi đầy năng lượng và có thể tương tác để kích hoạt kỹ năng đặc biệt.
    public class EnergyBarView : MonoBehaviour
    {
        [SerializeField] private Image _skillIcon;
        [SerializeField] private Image _fillBar;
        [SerializeField] private Image fillImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private GameObject fullEffect;

        [Header("Interaction")]
        [SerializeField] private Button skillButton;

        private EnergyDefinition _energyDef;
        public event Action<EnergyDefinition> OnSkillButtonClicked;

        private void Awake()
        {
            if (skillButton != null)
            {
                skillButton.onClick.AddListener(() => OnSkillButtonClicked?.Invoke(_energyDef));
                skillButton.interactable = false;
            }
        }

        public void Setup(EnergyDefinition def)
        {
            // FIX 3: Assign the _energyDef so it isn't null when invoked later
            _energyDef = def;

            // Hiển thị Icon của Skill được gắn trong EnergyDefinition
            if (def.grantedSkill != null)
            {
                _skillIcon.sprite = def.grantedSkill.icon;
            }
            _fillBar.color = def.uiColor; // Màu sắc đặc trưng của hệ
        }

        public void UpdateBar(int current, int max)
        {
            float ratio = (float)current / max;
            if (fillImage) fillImage.DOFillAmount(ratio, 0.3f).SetEase(Ease.OutCubic);

            if (valueText) valueText.text = $"{current}/{max}";

            if (current < max)
            {
                if (fullEffect) fullEffect.SetActive(false);
                if (skillButton) skillButton.interactable = false;

                skillButton.transform.DOKill();
                skillButton.transform.localScale = Vector3.one;
            }
        }

        public void ShowFullState()
        {
            if (fullEffect) fullEffect.SetActive(true);
            if (skillButton)
            {
                skillButton.interactable = true;

                skillButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0f), 0.5f, 5, 1f)
                    .SetLoops(-1, LoopType.Restart);
            }
        }
    }
}