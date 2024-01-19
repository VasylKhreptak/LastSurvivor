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
        float _normalizedSpeed;

        public void Tick() => Animate();

        private void Animate()
        {
            _velocity = _velocityAdapter.Value;
            _velocity.y = 0f;

            _normalizedSpeed = Mathf.Clamp01(_velocity.magnitude / _preferences.MaxSpeed);

            _animator.SetFloat(_preferences.SpeedParameterName, _normalizedSpeed);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _maxSpeed = 5f;
            [SerializeField] private string _speedParameterName = "Speed";

            public float MaxSpeed => _maxSpeed;
            public string SpeedParameterName => _speedParameterName;
        }
    }
}