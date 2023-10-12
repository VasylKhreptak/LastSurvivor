using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Jump
{
    [Serializable]
    public class JumpAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition;
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

            _transform.position = _startPosition;
        }

        public void SetEndState()
        {
            Stop();

            _transform.position = _endPosition;
        }

        public Tween CreateForwardTween() => CreateJumpTween(_endPosition);

        public Tween CreateBackwardTween() => CreateJumpTween(_startPosition);

        private Tween CreateJumpTween(Vector3 targetPosition)
        {
            return _transform
                .DOJump(targetPosition, _jumpPower, _jumpCount, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);
        }

        [Button]
        private void AssignStartPosition() => _startPosition = _transform.position;

        [Button]
        private void AssignEndPosition() => _endPosition = _transform.position;
    }
}
