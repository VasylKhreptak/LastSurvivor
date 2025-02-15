﻿using System;
using Providers.Velocity.Core;
using UnityEngine;
using Zenject;

namespace Entities.Animations
{
    public class PlaneMoveAnimation : ITickable
    {
        private readonly Transform _transform;
        private readonly Animator _animator;
        private readonly Preferences _preferences;
        private readonly IVelocityProvider _velocityProvider;

        public PlaneMoveAnimation(Transform transform, Animator animator, Preferences preferences, IVelocityProvider velocityProvider)
        {
            _transform = transform;
            _animator = animator;
            _preferences = preferences;
            _velocityProvider = velocityProvider;
        }

        private Vector3 _velocity;
        private Vector3 _viewDirection;
        private Vector3 _normalizedVelocity;
        private float _speed01;
        private Quaternion _rotationDifference;
        private Vector3 _planeInput;

        public void Tick() => Animate();

        private void Animate()
        {
            _velocity = _velocityProvider.Value;
            _velocity.y = 0f;
            _viewDirection = _transform.forward;
            _speed01 = Mathf.Min(_velocity.magnitude / _preferences.MaxSpeed, 1f);
            _normalizedVelocity = _velocity.normalized * _speed01;
            _rotationDifference = Quaternion.FromToRotation(_viewDirection, _normalizedVelocity) *
                                  Quaternion.FromToRotation(_normalizedVelocity, Vector3.forward);
            _planeInput = _rotationDifference * _normalizedVelocity;

            _animator.SetFloat(_preferences.HorizontalParameterName, _planeInput.x, _preferences.DampTime, Time.deltaTime);
            _animator.SetFloat(_preferences.VerticalParameterName, _planeInput.z, _preferences.DampTime, Time.deltaTime);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _maxSpeed = 5f;
            [SerializeField] private string _horizontalParameterName = "Horizontal";
            [SerializeField] private string _verticalParameterName = "Vertical";
            [SerializeField] private float _dampTime = 0.05f;

            public float MaxSpeed => _maxSpeed;
            public string HorizontalParameterName => _horizontalParameterName;
            public string VerticalParameterName => _verticalParameterName;
            public float DampTime => _dampTime;
        }
    }
}