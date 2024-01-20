using System;
using UnityEngine;
using Zenject;

namespace Noise
{
    public class NoiseMover : ITickable
    {
        private readonly Transform _transform;
        private readonly Preferences _preferences;

        private readonly Vector3 _initialLocalPosition;

        public NoiseMover(Transform transform, Preferences preferences)
        {
            _transform = transform;
            _preferences = preferences;

            _initialLocalPosition = _transform.localPosition;
        }

        public void Tick() => UpdatePosition();

        private void UpdatePosition()
        {
            if (Mathf.Approximately(_preferences.Amplitude, 0) || _preferences.Strength == Vector3.zero)
                return;

            float dx = (float)NoiseS3D.Noise(Time.time * _preferences.Speed, 0f, 0f) * _preferences.Amplitude * _preferences.Strength.x;
            float dy = (float)NoiseS3D.Noise(0f, Time.time * _preferences.Speed, 0f) * _preferences.Amplitude * _preferences.Strength.y;
            float dz = (float)NoiseS3D.Noise(0f, 0f, Time.time * _preferences.Speed) * _preferences.Amplitude * _preferences.Strength.z;

            Vector3 position = _initialLocalPosition;

            position.x += dx;
            position.y += dy;
            position.z += dz;

            _transform.localPosition = position;
        }

        [Serializable]
        public class Preferences
        {
            public float Speed = 1;
            public Vector3 Strength = Vector3.one;
            public float Amplitude = 1;
        }
    }
}