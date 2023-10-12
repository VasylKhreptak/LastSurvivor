using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Move
{
    [Serializable]
    public class MoveAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition;
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

        public Tween CreateForwardTween() => CreateMoveTween(_endPosition);

        public Tween CreateBackwardTween() => CreateMoveTween(_startPosition);

        private Tween CreateMoveTween(Vector3 targetPosition)
        {
            return _transform
                .DOMove(targetPosition, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);
        }

        [Button]
        private void AssignStartPosition() => _startPosition = _transform.position;

        [Button]
        private void AssignEndPosition() => _endPosition = _transform.position;
    }
}
