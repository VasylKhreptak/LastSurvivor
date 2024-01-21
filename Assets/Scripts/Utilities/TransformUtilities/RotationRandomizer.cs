using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities.TransformUtilities
{
    public class RotationRandomizer
    {
        private readonly Transform _transform;
        private readonly Preferences _preferences;

        public RotationRandomizer(Transform transform, Preferences preferences)
        {
            _transform = transform;
            _preferences = preferences;
        }

        public void Randomize()
        {
            _transform.rotation = Quaternion.Euler(Random.Range(_preferences.MinAngle.x, _preferences.MaxAngle.x),
                Random.Range(_preferences.MinAngle.y, _preferences.MaxAngle.y),
                Random.Range(_preferences.MinAngle.z, _preferences.MaxAngle.z));
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Vector3 _minAngle;
            [SerializeField] private Vector3 _maxAngle;

            public Vector3 MinAngle => _minAngle;
            public Vector3 MaxAngle => _maxAngle;
        }
    }
}