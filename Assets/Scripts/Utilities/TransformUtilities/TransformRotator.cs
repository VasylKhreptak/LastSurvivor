using System;
using UnityEngine;
using Zenject;

namespace Utilities.TransformUtilities
{
    public class TransformRotator : ITickable
    {
        private readonly Transform _transform;
        private readonly Preferences _preferences;

        public TransformRotator(Transform transform, Preferences preferences)
        {
            _transform = transform;
            _preferences = preferences;
        }

        public void Tick() => _transform.localRotation *= Quaternion.AngleAxis(_preferences.Speed * Time.deltaTime, _preferences.Axis);

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _speed;
            [SerializeField] private Vector3 _axis;

            public float Speed => _speed;
            public Vector3 Axis => _axis;
        }
    }
}