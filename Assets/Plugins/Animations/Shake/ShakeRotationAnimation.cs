using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Shake
{
    [Serializable]
    public class ShakeRotationAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector3 _baseLocalRotation;
        [SerializeField] private float _force = 1f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 90f;
        [SerializeField] private bool _fadeOut = true;
        [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;
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

            _transform.localEulerAngles = _baseLocalRotation;
        }

        public void SetEndState()
        {
            Stop();

            _transform.localEulerAngles = _baseLocalRotation;
        }

        public Tween CreateForwardTween() => CreateShakeRotationTween(_force);

        public Tween CreateBackwardTween() => CreateShakeRotationTween(-_force);

        private Tween CreateShakeRotationTween(float strength) =>
            _transform
                .DOShakeRotation(_duration, strength, _vibrato, _randomness, _fadeOut, _randomnessMode)
                .SetDelay(_delay)
                .SetEase(_curve);

        [Button]
        private void AssignBaseLocalRotation()
        {
            _baseLocalRotation = _transform.localEulerAngles;
        }
    }
}