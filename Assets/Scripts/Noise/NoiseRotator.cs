using System;
using UnityEngine;
using Zenject;

namespace Noise
{
    public class NoiseRotator : ITickable
    {
        private readonly Transform _transform;
        private readonly Preferences _preferences;

        private readonly Quaternion _initialLocalRotation;

        public NoiseRotator(Transform transform, Preferences preferences)
        {
            _transform = transform;
            _preferences = preferences;

            _initialLocalRotation = transform.localRotation;
        }

        public void Tick() => UpdateRotation();

        private void UpdateRotation()
        {
            if (Mathf.Approximately(_preferences.Amplitude, 0) || _preferences.Strength == Vector3.zero)
                return;

            float dx = (float)NoiseS3D.Noise(Time.time * _preferences.Speed, 0f, 0f) * _preferences.Amplitude * _preferences.Strength.x;
            float dy = (float)NoiseS3D.Noise(0f, Time.time * _preferences.Speed, 0f) * _preferences.Amplitude * _preferences.Strength.y;
            float dz = (float)NoiseS3D.Noise(0f, 0f, Time.time * _preferences.Speed) * _preferences.Amplitude * _preferences.Strength.z;

            Quaternion rotation = _initialLocalRotation;

            rotation *= Quaternion.AngleAxis(dx, _transform.right);
            rotation *= Quaternion.AngleAxis(dy, _transform.up);
            rotation *= Quaternion.AngleAxis(dz, _transform.forward);

            _transform.localRotation = rotation;
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