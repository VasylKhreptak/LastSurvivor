using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

namespace Utilities.SplineUtilities
{
    public class SplineTargetFollower : ITickable
    {
        private readonly Transform _transform;
        private readonly Transform _splineRoot;
        private readonly Preferences _preferences;
        private readonly Spline _spline;

        public SplineTargetFollower(Transform transform, SplineContainer splineContainer, Preferences preferences)
        {
            _transform = transform;
            _splineRoot = splineContainer.transform;
            _spline = splineContainer.Spline;
            _preferences = preferences;
        }

        private Vector3 _lastTargetPosition;
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        public Transform Target;

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
            SplineUtility.GetNearestPoint(_spline, localPoint, out float3 _, out float nearestTime);
            float targetSplineDistance = nearestTime * _spline.GetLength() + _preferences.DistanceOffset;
            Vector3 targetSplinePoint = _spline.GetPointAtLinearDistance(0, targetSplineDistance, out float _);
            _targetPosition = _splineRoot.TransformPoint(targetSplinePoint);
            _targetRotation = Quaternion.LookRotation(_spline.EvaluateTangent(nearestTime));
            _targetRotation *= Quaternion.Euler(_preferences.RotationOffset);
        }

        private void Move()
        {
            _transform.position = Vector3.Lerp(_transform.position, _targetPosition, Time.deltaTime * _preferences.FollowSpeed);
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
            [SerializeField] private Vector3 _rotationOffset = Vector3.zero;

            public float FollowSpeed => _followSpeed;
            public float RotateSpeed => _rotateSpeed;
            public float DistanceOffset => _distanceOffset;
            public Vector3 RotationOffset => _rotationOffset;
        }
    }
}