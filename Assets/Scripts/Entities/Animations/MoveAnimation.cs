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
        private float _previousNormalizedSpeed;
        private float _normalizedSpeed;

        public void Tick() => Animate();

        private void Animate()
        {
            _velocity = _velocityAdapter.Value;
            _velocity.y = 0f;

            _normalizedSpeed = Mathf.Lerp(_previousNormalizedSpeed, Mathf.Clamp01(_velocity.magnitude / _preferences.MaxSpeed),
                Time.deltaTime * _preferences.SmoothTime);

            _animator.SetFloat(_preferences.SpeedParameterName, _normalizedSpeed);

            _previousNormalizedSpeed = _normalizedSpeed;
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