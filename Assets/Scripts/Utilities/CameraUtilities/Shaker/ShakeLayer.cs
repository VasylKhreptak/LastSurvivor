using System;
using DG.Tweening;
using Noise;
using UnityEngine;
using Zenject;

namespace Utilities.CameraUtilities.Shaker
{
    public class ShakeLayer : ITickable, IDisposable
    {
        private readonly Transform _transform;
        private readonly Preferences _preferences;

        private readonly NoiseRotator _noiseRotator;

        public ShakeLayer(Transform transform, Preferences preferences)
        {
            _transform = transform;
            _preferences = preferences;

            _noiseRotator = new NoiseRotator(_transform, _preferences.NoisePreferences);
        }

        private Sequence _amplitudeSequence;

        public void Dispose()
        {
            _amplitudeSequence?.Kill();
            _preferences.NoisePreferences.Amplitude = 0f;
        }

        public void Tick() => _noiseRotator.Tick();

        public void Shake()
        {
            _amplitudeSequence?.Kill();

            _amplitudeSequence = DOTween.Sequence();
            _amplitudeSequence
                .Append(DOTween.To(GetAmplitude, SetAmplitude, _preferences.TargetAmplitude, _preferences.ShakeUpDuration)
                    .SetEase(_preferences.ShakeUpCurve))
                .AppendInterval(_preferences.ShakeDuration)
                .Append(DOTween.To(GetAmplitude, SetAmplitude, 0f, _preferences.ShakeDownDuration)
                    .SetEase(_preferences.ShakeDownCurve))
                .Play();
        }

        private float GetAmplitude() => _preferences.NoisePreferences.Amplitude;

        private void SetAmplitude(float amplitude) => _preferences.NoisePreferences.Amplitude = amplitude;

        [Serializable]
        public class Preferences
        {
            [SerializeField] private NoiseRotator.Preferences _noisePreferences;

            [SerializeField] private float _shakeUpDuration = 0.2f;
            [SerializeField] private float _shakeDuration;
            [SerializeField] private float _shakeDownDuration = 0.4f;
            [SerializeField] private float _targetAmplitude = 1f;
            [SerializeField] private AnimationCurve _shakeUpCurve;
            [SerializeField] private AnimationCurve _shakeDownCurve;

            public NoiseRotator.Preferences NoisePreferences => _noisePreferences;

            public float ShakeUpDuration => _shakeUpDuration;
            public float ShakeDuration => _shakeDuration;
            public float ShakeDownDuration => _shakeDownDuration;
            public float TargetAmplitude => _targetAmplitude;
            public AnimationCurve ShakeUpCurve => _shakeUpCurve;
            public AnimationCurve ShakeDownCurve => _shakeDownCurve;
        }
    }
}