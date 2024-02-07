﻿using System;
using DG.Tweening;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Punch
{
    [Serializable]
    public class PunchPositionAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Vector3 _baseLocalPosition;
        [SerializeField] private float _force = 1f;
        [SerializeField] private Vector3 _direction = Vector3.up;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _elasticity = 1f;
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

            _transform.localPosition = _baseLocalPosition;
        }

        public void SetEndState()
        {
            Stop();

            _transform.localPosition = _baseLocalPosition;
        }

        public Tween CreateForwardTween() => CreatePunchPositionTween(_direction * _force);

        public Tween CreateBackwardTween() => CreatePunchPositionTween(-_direction * _force);

        private Tween CreatePunchPositionTween(Vector3 punch) =>
            _transform
                .DOPunchPosition(punch, _duration, _vibrato, _elasticity)
                .SetDelay(_delay)
                .SetEase(_curve);

        [Button]
        private void AssignBaseLocalPosition()
        {
            _baseLocalPosition = _transform.localPosition;
        }
    }
}