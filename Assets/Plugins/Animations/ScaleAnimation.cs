using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations
{
    [Serializable]
    public class ScaleAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
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

            _transform.localScale = _startScale;
        }

        public void SetEndState()
        {
            Stop();

            _transform.localScale = _endScale;
        }

        public Tween CreateForwardTween() => CreateScaleTween(_endScale);

        public Tween CreateBackwardTween() => CreateScaleTween(_startScale);

        private Tween CreateScaleTween(Vector3 targetScale) =>
            _transform
                .DOScale(targetScale, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);

        [Button]
        private void AssignStartScale() => _endScale = _transform.localScale;

        [Button]
        private void AssignEndScale() => _endScale = _transform.localScale;
    }
}