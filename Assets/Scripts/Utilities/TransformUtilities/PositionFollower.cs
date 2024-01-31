using System;
using UnityEngine;
using Zenject;

namespace Utilities.TransformUtilities
{
    public class PositionFollower : ITickable
    {
        private readonly Transform _origin;
        private readonly Transform _target;
        private readonly Preferences _preferences;

        public PositionFollower(Transform origin, Transform target, Preferences preferences)
        {
            _origin = origin;
            _target = target;
            _preferences = preferences;
        }

        public void Tick() => UpdatePosition();

        private void UpdatePosition()
        {
            _origin.position = _target.position + _preferences.Offset;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Vector3 _offset;

            public Vector3 Offset => _offset;
        }
    }
}