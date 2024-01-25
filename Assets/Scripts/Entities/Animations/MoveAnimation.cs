using System;
using Adapters.Velocity.Core;
using UnityEngine;
using Zenject;

namespace Entities.Animations
{
    public class MoveAnimation : ITickable
    {
        private readonly Animator _animator;
        private readonly Preferences _preferences;
        private readonly IVelocityAdapter _velocityAdapter;

        public MoveAnimation(Animator animator, Preferences preferences, IVelocityAdapter velocityAdapter)
        {
            _animator = animator;
            _preferences = preferences;
            _velocityAdapter = velocityAdapter;
        }

        private Vector3 _velocity;
        private float _previousSpeed01;
        private float _speed01;

        public void Tick() => Animate();

        private void Animate()
        {
            _velocity = _velocityAdapter.Value;
            _velocity.y = 0f;

            _speed01 = Mathf.Lerp(_previousSpeed01, Mathf.Clamp01(_velocity.magnitude / _preferences.MaxSpeed),
                Time.deltaTime * _preferences.SmoothTime);

            _animator.SetFloat(_preferences.SpeedParameterName, _speed01);

            _previousSpeed01 = _speed01;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _maxSpeed = 5f;
            [SerializeField] private string _speedParameterName = "Speed";
            [SerializeField] private float _smoothTime = 5f;

            public float MaxSpeed => _maxSpeed;
            public string SpeedParameterName => _speedParameterName;
            public float SmoothTime => _smoothTime;
        }
    }
}