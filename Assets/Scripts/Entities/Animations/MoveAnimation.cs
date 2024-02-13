using System;
using Providers.Velocity.Core;
using UnityEngine;
using Zenject;

namespace Entities.Animations
{
    public class MoveAnimation : ITickable
    {
        private readonly Animator _animator;
        private readonly Preferences _preferences;
        private readonly IVelocityProvider _velocityProvider;

        public MoveAnimation(Animator animator, Preferences preferences, IVelocityProvider velocityProvider)
        {
            _animator = animator;
            _preferences = preferences;
            _velocityProvider = velocityProvider;
        }

        private Vector3 _velocity;
        private float _previousSpeed01;
        private float _speed01;

        public void Tick() => Animate();

        private void Animate()
        {
            _velocity = _velocityProvider.Value;
            _velocity.y = 0f;

            _speed01 = Mathf.Min(_velocity.magnitude / _preferences.MaxSpeed, 1f);

            _animator.SetFloat(_preferences.SpeedParameterName, _speed01, _preferences.DampTime, Time.deltaTime);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _maxSpeed = 5f;
            [SerializeField] private string _speedParameterName = "Speed";
            [SerializeField] private float _dampTime = 0.05f;

            public float MaxSpeed => _maxSpeed;
            public string SpeedParameterName => _speedParameterName;
            public float DampTime => _dampTime;
        }
    }
}