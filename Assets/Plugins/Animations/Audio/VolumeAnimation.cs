using System;
using DG.Tweening;
using Plugins.Animations.Adapters.Volume.Core;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.Animations.Audio
{
    [Serializable]
    public class VolumeAnimation : IAnimation
    {
        [Header("References")]
        [SerializeField] private VolumeAdapter _volumeAdapter;

        [Header("Preferences")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;
        [SerializeField] private float _startVolume;
        [SerializeField] private float _endVolume;
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

            _volumeAdapter.Value = _startVolume;
        }

        public void SetEndState()
        {
            Stop();

            _volumeAdapter.Value = _endVolume;
        }

        public Tween CreateForwardTween() => CreateVolumeTween(_endVolume);

        public Tween CreateBackwardTween() => CreateVolumeTween(_startVolume);

        private Tween CreateVolumeTween(float alpha)
        {
            return DOTween
                .To(() => _volumeAdapter.Value, x => _volumeAdapter.Value = x, alpha, _duration)
                .SetDelay(_delay)
                .SetEase(_curve);
        }

        [Button]
        private void AssignStartVolume() => _startVolume = _volumeAdapter.Value;

        [Button]
        private void AssignEndVolume() => _endVolume = _volumeAdapter.Value;
    }
}