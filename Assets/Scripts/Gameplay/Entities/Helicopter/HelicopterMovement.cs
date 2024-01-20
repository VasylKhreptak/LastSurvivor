using System;
using Gameplay.Entities.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Helicopter
{
    public class HelicopterMovement : ITickable
    {
        private readonly Transform _transform;
        private readonly PlayerHolder _playerHolder;
        private readonly Preferences _preferences;

        public HelicopterMovement(Transform transform, Preferences preferences)
        {
            _transform = transform;
            _preferences = preferences;
        }

        public void Tick() { }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _followSpeed = 1f;
            [SerializeField] private float _distanceOffset = 5f;

            public float FollowSpeed => _followSpeed;
            public float DistanceOffset => _distanceOffset;
        }
    }
}