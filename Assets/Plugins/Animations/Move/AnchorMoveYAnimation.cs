using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Move
{
    [Serializable]
    public class AnchorMoveYAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private float _startY;
        [SerializeField] private float _endY;
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

            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _startY);
        }

        public void SetEndState()
        {
            Stop();

            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _endY);
        }

        public Tween CreateForwardTween() => CreateMoveTween(_endY);

        public Tween CreateBackwardTween() => CreateMoveTween(_startY);

        private Tween CreateMoveTween(float targetY)
        {
            return _rectTransform
                .DOAnchorPosY(targetY, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);
        }

        [Button]
        private void AssignStartPosition() => _startY = _rectTransform.anchoredPosition.y;

        [Button]
        private void AssignEndPosition() => _endY = _rectTransform.anchoredPosition.y;
    }
}
