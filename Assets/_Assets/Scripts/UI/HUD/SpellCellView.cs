using _Assets.Scripts.Core.Infrastructure.Constant;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Assets.Scripts.UI.HUD
{
    public class SpellCellView : MonoBehaviour
    {
        [SerializeField] private Image spellImage;
        [SerializeField] private Image spellCooldown;
        [SerializeField] private Image selectedFrame;
        
        private bool _isSelected;

        public void Initialize(Sprite spellIcon, bool isSelected)
        {
            spellImage.sprite = spellIcon;
            spellCooldown.fillAmount = 0;

            SetSelected(isSelected);
            gameObject.SetActive(true);
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            selectedFrame.gameObject.SetActive(isSelected);
        }

        public void StartCooldown(int cooldownInMillis)
        {
            var cooldownInSeconds = (float) cooldownInMillis / Constants.MillisInSecond;
            
            spellCooldown.DOKill();
            spellCooldown.fillAmount = 1f;
            spellCooldown
                .DOFillAmount(0f, cooldownInSeconds)
                .SetEase(Ease.Linear);
        }
    }
}