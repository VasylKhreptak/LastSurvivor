using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Jump
{
    [Serializable]
    public class AnchorJumpAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector2 _startAnchoredPosition;
        [SerializeField] private Vector2 _endAnchoredPosition;
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private int _jumpCount = 1;
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

            _rectTransform.anchoredPosition = _startAnchoredPosition;
        }

        public void SetEndState()
        {
            Stop();

            _rectTransform.anchoredPosition = _endAnchoredPosition;
        }

        public Tween CreateForwardTween() => CreateAnchorJumpTween(_endAnchoredPosition);

        public Tween CreateBackwardTween() => CreateAnchorJumpTween(_startAnchoredPosition);

        private Tween CreateAnchorJumpTween(Vector2 targetPosition) =>
            _rectTransform
                .DOJumpAnchorPos(targetPosition, _jumpPower, _jumpCount, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);

        [Button]
        private void AssignStartAnchoredPosition() => _startAnchoredPosition = _rectTransform.anchoredPosition;

        [Button]
        private void AssignEndLocalAnchoredPosition() => _endAnchoredPosition = _rectTransform.anchoredPosition;
    }
}