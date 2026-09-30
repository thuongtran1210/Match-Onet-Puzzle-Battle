using UnityEngine;
using UnityEngine.UI;
using System;
using BeastLinkBattle.Gameplay.Level;
using TMPro;

namespace BeastLinkBattle.UI.Views
{

    // View cho từng node trong màn hình chọn level, hiển thị số level, trạng thái khóa/mở khóa và số sao đạt được.
    public class LevelNodeItemView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _nodeBgImage;
        [SerializeField] private GameObject[] _stars; 
        [Header("Assets")]
        [SerializeField] private Sprite _unlockedSprite; 
        [SerializeField] private Sprite _lockedSprite;   

        public void Setup(LevelData data, bool isUnlocked, int starCount, Action<LevelData> onClick)
        {
            if (_levelText != null) _levelText.text = data.levelId.ToString();

            _button.interactable = isUnlocked;
            _nodeBgImage.sprite = isUnlocked ? _unlockedSprite : _lockedSprite;

            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].SetActive(isUnlocked && i < starCount);
            }

            _button.onClick.RemoveAllListeners();
            if (isUnlocked)
            {
                _button.onClick.AddListener(() => onClick?.Invoke(data));
            }
        }
    }
}