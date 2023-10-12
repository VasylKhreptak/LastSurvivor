using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations
{
    [Serializable]
    public class SizeDeltaAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector2 _startSizeDelta;
        [SerializeField] private Vector2 _endSizeDelta;
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

            _rectTransform.sizeDelta = _startSizeDelta;
        }

        public void SetEndState()
        {
            Stop();

            _rectTransform.sizeDelta = _endSizeDelta;
        }

        public Tween CreateForwardTween() => CreateSizeDeltaTween(_endSizeDelta);

        public Tween CreateBackwardTween() => CreateSizeDeltaTween(_startSizeDelta);

        private Tween CreateSizeDeltaTween(Vector2 targetSizeDelta)
        {
            return _rectTransform
                .DOSizeDelta(targetSizeDelta, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);
        }

        [Button]
        private void AssignStartSizeDelta() => _startSizeDelta = _rectTransform.sizeDelta;

        [Button]
        private void AssignEndSizeDelta() => _endSizeDelta = _rectTransform.sizeDelta;
    }
}
