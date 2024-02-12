using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities.PhysicsUtilities.RigidbodyUtilities
{
    public class RotationRandomizer
    {
        private readonly Rigidbody _rigidbody;
        private readonly Preferences _preferences;

        public RotationRandomizer(Rigidbody rigidbody, Preferences preferences)
        {
            _rigidbody = rigidbody;
            _preferences = preferences;
        }

        public void Randomize()
        {
            _rigidbody.rotation = Quaternion.Euler(Random.Range(_preferences.MinAngle.x, _preferences.MaxAngle.x),
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