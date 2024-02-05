using System;
using Tags.Gameplay;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;
using Zenject;

namespace Gameplay.Entities.Soldier
{
    public class SoldierAimer : ITickable
    {
        private readonly Transform _transform;
        private readonly ClosestTriggerObserver<SoldierTarget> _closestTargetObserver;
        private readonly Preferences _preferences;

        public SoldierAimer(Transform transform, ClosestTriggerObserver<SoldierTarget> closestTargetObserver, Preferences preferences)
        {
            _transform = transform;
            _closestTargetObserver = closestTargetObserver;
            _preferences = preferences;
        }

        private Quaternion _targetRotation;

        private Vector3 _lookPoint;

        private bool _isTargetVisible;

        private float _timeFromLastRaycast = 0f;

        private readonly BoolReactiveProperty _isAimed = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsAimed => _isAimed;

        public bool Enabled = false;

        public void Tick()
        {
            RaycastToTarget();
            UpdateTargetRotation();
            Aim();
            UpdateIsAimedProperty();
        }

        private void RaycastToTarget()
        {
            _timeFromLastRaycast += Time.deltaTime;

            if (_timeFromLastRaycast < _preferences.RaycastInterval)
                return;

            _timeFromLastRaycast = 0f;

            if (_closestTargetObserver.Trigger.Value == null || Enabled == false)
            {
                _isTargetVisible = false;
                return;
            }

            Vector3 headPosition = _preferences.HeadTransform.position;
            Vector3 targetPosition = _closestTargetObserver.Trigger.Value.Transform.position;
            targetPosition.y = headPosition.y;
            Vector3 direction = targetPosition - headPosition;
            Ray ray = new Ray(headPosition, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, _preferences.ViewLayerMask) == false)
                return;

            _isTargetVisible = hit.transform == _closestTargetObserver.Trigger.Value.Transform;
        }

        private void UpdateTargetRotation()
        {
            if (_closestTargetObserver.Trigger.Value == null || Enabled == false || _isTargetVisible == false)
            {
                _targetRotation = _transform.parent.rotation;
                return;
            }

            _lookPoint = _closestTargetObserver.Trigger.Value.Transform.position;
            _lookPoint.y = _transform.position.y;
            _targetRotation = Quaternion.LookRotation(_lookPoint - _transform.position);
        }

        private void Aim() =>
            _transform.rotation = Quaternion.Lerp(_transform.rotation, _targetRotation, _preferences.Speed * Time.deltaTime);

        private void UpdateIsAimedProperty()
        {
            if (_closestTargetObserver.Trigger.Value == null || Enabled == false || _isTargetVisible == false)
            {
                _isAimed.Value = false;
                return;
            }

            _isAimed.Value = Quaternion.Angle(_transform.rotation, _targetRotation) < _preferences.MaxAngleThreshold;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _headTransform;
            [SerializeField] private float _speed = 20f;
            [SerializeField] private float _maxAngleThreshold = 10f;
            [SerializeField] private LayerMask _viewLayerMask;
            [SerializeField] private float _raycastInterval = 1 / 4f;

            public Transform HeadTransform => _headTransform;
            public float Speed => _speed;
            public float MaxAngleThreshold => _maxAngleThreshold;
            public LayerMask ViewLayerMask => _viewLayerMask;
            public float RaycastInterval => _raycastInterval;
        }
    }
}