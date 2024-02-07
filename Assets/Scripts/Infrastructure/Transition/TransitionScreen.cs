using System;
using DG.Tweening;
using Infrastructure.Transition.Core;
using UniRx;
using UnityEngine;

namespace Infrastructure.Transition
{
    public class TransitionScreen : MonoBehaviour, ITransitionScreen
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _curve;

        private Tween _tween;

        private readonly FloatReactiveProperty _fadeProgress = new FloatReactiveProperty(1f);

        public IReadOnlyReactiveProperty<float> FadeProgress => _fadeProgress;

        #region MonoBehaviour

        private void OnDestroy() => _tween?.Kill();

        #endregion

        public void Show(Action onComplete = null)
        {
            gameObject.SetActive(true);

            _tween?.Kill();
            _tween = _canvasGroup
                .DOFade(1f, _duration)
                .SetEase(_curve)
                .OnUpdate(() => _fadeProgress.Value = 1 - _canvasGroup.alpha)
                .OnComplete(() =>
                {
                    _fadeProgress.Value = 0f;
                    onComplete?.Invoke();
                })
                .Play();
        }

        public void Hide(Action onComplete = null)
        {
            _tween?.Kill();
            _tween = _canvasGroup
                .DOFade(0f, _duration)
                .SetEase(_curve)
                .OnUpdate(() => _fadeProgress.Value = 1 - _canvasGroup.alpha)
                .OnComplete(() =>
                {
                    _fadeProgress.Value = 1;
                    gameObject.SetActive(false);
                    onComplete?.Invoke();
                })
                .Play();
        }

        public void ShowImmediately()
        {
            _tween?.Kill();
            _canvasGroup.alpha = 1f;
            gameObject.SetActive(true);
        }

        public void HideImmediately()
        {
            _tween?.Kill();
            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }
    }
}