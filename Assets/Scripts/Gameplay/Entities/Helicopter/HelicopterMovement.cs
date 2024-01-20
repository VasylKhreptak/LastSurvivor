using System;
using Gameplay.Entities.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

namespace Gameplay.Entities.Helicopter
{
    public class HelicopterMovement : ITickable
    {
        private readonly Transform _transform;
        private readonly PlayerHolder _playerHolder;
        private readonly Preferences _preferences;

        private readonly Spline _spline;

        public HelicopterMovement(Transform transform, PlayerHolder playerHolder, Preferences preferences)
        {
            _transform = transform;
            _playerHolder = playerHolder;
            _preferences = preferences;
            _spline = _preferences.SplineContainer.Spline;
        }

        private Vector3 _lastPlayerPosition;
        private Vector3 _targetPosition;

        private Quaternion _targetRotation;

        public void Tick()
        {
            UpdateLastPlayerPosition();
            UpdateTargetValues();
            MoveHelicopter();
        }

        private void UpdateLastPlayerPosition()
        {
            if (_playerHolder.Instance == null)
                return;

            _lastPlayerPosition = _playerHolder.Instance.transform.position;
        }

        private void UpdateTargetValues()
        {
            Vector3 localPoint = _preferences.SplineContainer.transform.InverseTransformPoint(_lastPlayerPosition);
            SplineUtility.GetNearestPoint(_spline, localPoint, out float3 _, out float nearestTime);
            float targetSplineDistance = nearestTime * _spline.GetLength() + _preferences.DistanceOffset;
            Vector3 targetSplinePoint = _spline.GetPointAtLinearDistance(0, targetSplineDistance, out float _);
            _targetPosition = _preferences.SplineContainer.transform.TransformPoint(targetSplinePoint);
            _targetRotation = Quaternion.LookRotation(_spline.EvaluateTangent(nearestTime));
        }

        private void MoveHelicopter()
        {
            _transform.position = Vector3.Lerp(_transform.position, _targetPosition, Time.deltaTime * _preferences.FollowSpeed);
            _transform.rotation = Quaternion.Lerp(_transform.rotation, _targetRotation * Quaternion.Euler(_preferences.RotationOffset),
                Time.deltaTime * _preferences.RotateSpeed);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private SplineContainer _splineContainer;
            [SerializeField] private float _followSpeed = 1f;
            [SerializeField] private float _rotateSpeed = 1f;
            [SerializeField] private float _distanceOffset = -5f;
            [SerializeField] private Vector3 _rotationOffset = Vector3.zero;

            public SplineContainer SplineContainer => _splineContainer;
            public float FollowSpeed => _followSpeed;
            public float RotateSpeed => _rotateSpeed;
            public float DistanceOffset => _distanceOffset;
            public Vector3 RotationOffset => _rotationOffset;
        }
    }
}