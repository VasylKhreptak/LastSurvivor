using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Rotate
{
    [Serializable]
    public class LocalRotateAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector3 _startLocalRotation;
        [SerializeField] private Vector3 _endLocalRotation;
        [SerializeField] private RotateMode _rotateMode = RotateMode.Fast;
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

            _transform.localEulerAngles = _startLocalRotation;
        }

        public void SetEndState()
        {
            Stop();

            _transform.localEulerAngles = _endLocalRotation;
        }

        public Tween CreateForwardTween() => CreateLocalRotateTween(_endLocalRotation);

        public Tween CreateBackwardTween() => CreateLocalRotateTween(_startLocalRotation);

        private Tween CreateLocalRotateTween(Vector3 targetRotation) =>
            _transform
                .DOLocalRotate(targetRotation, _duration, _rotateMode)
                .SetDelay(_delay)
                .SetEase(_curve);

        [Button]
        private void AssignStartRotation() => _startLocalRotation = _transform.localEulerAngles;

        [Button]
        private void AssignEndRotation() => _endLocalRotation = _transform.localEulerAngles;
    }
}