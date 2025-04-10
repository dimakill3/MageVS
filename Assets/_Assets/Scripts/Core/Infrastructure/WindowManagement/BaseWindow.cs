using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _Assets.Scripts.Core.Infrastructure.WindowManagement
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class BaseWindow : MonoBehaviour
    {
        public event Action<BaseWindow> Showed;
        public event Action<BaseWindow> Hided;

        [SerializeField] private AnimationClip showAnimation;
        [SerializeField] private AnimationClip hideAnimation;
        [SerializeField] private Animation animationComponent;
        [SerializeField] protected List<Button> closeButtons;

        private bool _isShowed;
        
        public RectTransform RectTransform => (RectTransform)gameObject.transform;
        public bool IsShowed => _isShowed;

        public virtual void Show(bool animated = true)
        {
            if (_isShowed)
                return;

            gameObject.SetActive(true);
            _isShowed = true;
            Showed?.Invoke(this);

            if (showAnimation != null && animated)
                PlayAnimation(showAnimation, OnEndShowAnimation).Forget();
            else
                OnEndShowAnimation();
        }

        public virtual void Hide(bool animated = true)
        {
            if (!_isShowed)
                return;

            if (hideAnimation != null && animated)
                PlayAnimation(hideAnimation, OnEndHideAnimation).Forget();
            else
                OnEndHideAnimation();
        }

        protected virtual void Start()
        {
            foreach (var closeButton in closeButtons)
                closeButton.onClick.AddListener(OnClickCloseButton);
        }

        protected virtual void OnDestroy()
        {
            foreach (var closeButton in closeButtons)
                closeButton.onClick.RemoveListener(OnClickCloseButton);
        }

        protected virtual void OnEndShowAnimation()
        {
        }

        protected virtual void OnEndHideAnimation()
        {
            gameObject.SetActive(false);
            _isShowed = false;
            
            Hided?.Invoke(this);
        }

        protected virtual void OnClickCloseButton() =>
            Hide();

        private async UniTask PlayAnimation(AnimationClip animationClip, Action action = null)
        {
            animationComponent.Play(animationClip.name, PlayMode.StopAll);
            await UniTask.WaitForEndOfFrame();

            while (animationComponent.IsPlaying(animationClip.name))
                await UniTask.WaitForEndOfFrame();
            
            action?.Invoke();
        }
    }
}