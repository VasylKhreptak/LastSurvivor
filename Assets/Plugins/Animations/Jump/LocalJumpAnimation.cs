using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Jump
{
    [Serializable]
    public class LocalJumpAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector3 _startLocalPosition;
        [SerializeField] private Vector3 _endLocalPosition;
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

            _transform.localPosition = _startLocalPosition;
        }

        public void SetEndState()
        {
            Stop();

            _transform.localPosition = _endLocalPosition;
        }

        public Tween CreateForwardTween() => CreateLocalJumpTween(_endLocalPosition);

        public Tween CreateBackwardTween() => CreateLocalJumpTween(_startLocalPosition);

        private Tween CreateLocalJumpTween(Vector3 targetPosition)
        {
            return _transform
                .DOLocalJump(targetPosition, _jumpPower, _jumpCount, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);
        }

        [Button]
        private void AssignStartLocalPosition() => _startLocalPosition = _transform.localPosition;

        [Button]
        private void AssignEndLocalPosition() => _endLocalPosition = _transform.localPosition;
    }
}
