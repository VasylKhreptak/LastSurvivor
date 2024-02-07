using System;
using DG.Tweening;
using Plugins.Animations.Adapters.Alpha.Core;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations
{
    [Serializable]
    public class FadeAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private AlphaAdapter _alphaAdapter;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        private Tween _tween;

        public float Duration => _duration;

        public float Delay => _delay;

        public bool IsPlaying => _tween != null && _tween.IsPlaying();

        public void PlayForward(Action onComplete = null)
        {
            Stop();
            _tween = CreateForwardTween().OnComplete(() => onComplete?.Invoke()).Play();
        }

        public void PlayBackward(Action onComplete = null)
        {
            Stop();
            _tween = CreateBackwardTween().OnComplete(() => onComplete?.Invoke()).Play();
        }

        public void Stop() => _tween.Kill();

        public void SetStartState()
        {
            Stop();

            _alphaAdapter.Value = _startAlpha;
        }

        public void SetEndState()
        {
            Stop();

            _alphaAdapter.Value = _endAlpha;
        }

        public Tween CreateForwardTween() => CreateFadeTween(_endAlpha);

        public Tween CreateBackwardTween() => CreateFadeTween(_startAlpha);

        private Tween CreateFadeTween(float alpha)
        {
            return DOTween
                .To(() => _alphaAdapter.Value, x => _alphaAdapter.Value = x, alpha, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);
        }

        [Button]
        private void AssignStartAlpha() => _startAlpha = _alphaAdapter.Value;

        [Button]
        private void AssignEndAlpha() => _endAlpha = _alphaAdapter.Value;
    }
}