using System;
using DG.Tweening;
using Infrastructure.Curtain.Core;
using UnityEngine;

namespace Infrastructure.Curtain
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _delay = 0.3f;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        private Tween _moveTween;

        public event Action OnHidden;

        #region MonoBehaviour

        private void OnDestroy()
        {
            KillTween();
        }

        #endregion

        public void Show()
        {
            KillTween();

            _rectTransform.anchoredPosition = Vector2.zero;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (gameObject.activeSelf == false) return;

            KillTween();

            _moveTween = _rectTransform
                .DOAnchorPosY(_rectTransform.rect.height, _duration)
                .SetDelay(_delay)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    OnHidden?.Invoke();
                })
                .SetEase(_ease)
                .Play();
        }

        private void KillTween()
        {
            _moveTween.Kill();
        }
    }
}
