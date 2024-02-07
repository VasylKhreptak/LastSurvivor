using System;
using DG.Tweening;
using Plugins.Animations.Adapters.Color.Core;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations
{
    [Serializable]
    public class ColorAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private ColorAdapter _colorAdapter;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;
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

            _colorAdapter.Value = _startColor;
        }

        public void SetEndState()
        {
            Stop();

            _colorAdapter.Value = _endColor;
        }

        public Tween CreateForwardTween() => CreateColorTween(_endColor);

        public Tween CreateBackwardTween() => CreateColorTween(_startColor);

        private Tween CreateColorTween(Color color)
        {
            return DOTween
                .To(() => _colorAdapter.Value, x => _colorAdapter.Value = x, color, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);
        }

        [Button]
        private void AssignStartColor() => _startColor = _colorAdapter.Value;

        [Button]
        private void AssignEndColor() => _endColor = _colorAdapter.Value;
    }
}