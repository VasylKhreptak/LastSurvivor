using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Utilities.SplineUtilities.TargetFollower.Core;
using Zenject;

namespace Utilities.SplineUtilities.TargetFollower
{
    public class SplineTargetFollower : ISplineTargetFollower, ITickable
    {
        private readonly Transform _transform;
        private readonly Transform _splineRoot;
        private readonly Preferences _preferences;
        private readonly Spline _spline;
        private readonly float _splineLength;

        public SplineTargetFollower(Transform transform, SplineContainer splineContainer, Preferences preferences)
        {
            _transform = transform;
            _splineRoot = splineContainer.transform;
            _spline = splineContainer.Spline;
            _preferences = preferences;
            _splineLength = _spline.GetLength();
        }

        private Vector3 _lastTargetPosition;
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        private float _nearestTime;
        private float _positionTime;
        private float _rotationTime;
        private float3 _position;
        private float3 _tangent;
        private float3 _up;

        public Transform Target { get; set; }

        public void Tick()
        {
            UpdateLastTargetPosition();
            UpdateTargetValues();
            Move();
        }

        private void UpdateLastTargetPosition()
        {
            if (Target == null)
                return;

            _lastTargetPosition = Target.position;
        }

        private void UpdateTargetValues()
        {
            Vector3 localPoint = _splineRoot.InverseTransformPoint(_lastTargetPosition);
            SplineUtility.GetNearestPoint(_spline, localPoint, out float3 _, out _nearestTime);
            _positionTime = math.clamp(_nearestTime + _preferences.DistanceOffset / _splineLength, 0f, 1f);
            _rotationTime = math.clamp(_nearestTime + _preferences.RotationDistanceOffset / _splineLength, 0f, 1f);
            _position = _spline.EvaluatePosition(_positionTime);
            _tangent = _spline.EvaluateTangent(_rotationTime);
            _up = _spline.EvaluateUpVector(_rotationTime);
            _targetPosition = _splineRoot.TransformPoint(_position);
            _targetRotation = Quaternion.LookRotation(_tangent, _up);
            _targetRotation *= Quaternion.Euler(_preferences.RotationOffset);
        }

        private void Move()
        {
            _transform.position = Vector3.Lerp(_transform.position, _targetPosition, Time.deltaTime * _preferences.FollowSpeed);

            if (_preferences.UpdateRotation == false)
                return;

            _transform.rotation = Quaternion.Lerp(_transform.rotation, _targetRotation, Time.deltaTime * _preferences.RotateSpeed);
        }

        public void FollowTargetImmediately()
        {
            UpdateLastTargetPosition();
            UpdateTargetValues();
            MoveImmediately();
        }

        private void MoveImmediately()
        {
            _transform.position = _targetPosition;
            _transform.rotation = _targetRotation;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _followSpeed = 1f;
            [SerializeField] private float _rotateSpeed = 1f;
            [SerializeField] private float _distanceOffset = -5f;
            [SerializeField] private float _rotationDistanceOffset = 0f;
            [SerializeField] private Vector3 _rotationOffset = Vector3.zero;
            [SerializeField] private bool _updateRotation = true;

            public float FollowSpeed => _followSpeed;
            public float RotateSpeed => _rotateSpeed;
            public float DistanceOffset => _distanceOffset;
            public float RotationDistanceOffset => _rotationDistanceOffset;
            public Vector3 RotationOffset => _rotationOffset;
            public bool UpdateRotation => _updateRotation;
        }
    }
}