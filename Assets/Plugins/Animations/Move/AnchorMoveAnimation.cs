using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Move
{
    [Serializable]
    public class AnchorMoveAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector2 _startPosition;
        [SerializeField] private Vector2 _endPosition;
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

            _rectTransform.anchoredPosition = _startPosition;
        }

        public void SetEndState()
        {
            Stop();

            _rectTransform.anchoredPosition = _endPosition;
        }

        public Tween CreateForwardTween() => CreateMoveTween(_endPosition);

        public Tween CreateBackwardTween() => CreateMoveTween(_startPosition);

        private Tween CreateMoveTween(Vector2 targetPosition)
        {
            return _rectTransform
                .DOAnchorPos(targetPosition, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);
        }

        [Button]
        private void AssignStartPosition() => _startPosition = _rectTransform.anchoredPosition;

        [Button]
        private void AssignEndPosition() => _endPosition = _rectTransform.anchoredPosition;
    }
}
