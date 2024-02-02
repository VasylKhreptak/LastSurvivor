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

        private BoolReactiveProperty _isAimed = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsAimed => _isAimed;

        public bool Enabled = false;

        public void Tick()
        {
            UpdateTargetRotation();
            Aim();
            UpdateIsAimerProperty();
        }

        private void UpdateTargetRotation()
        {
            if (_closestTargetObserver.Trigger.Value == null || Enabled == false)
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

        private void UpdateIsAimerProperty()
        {
            if (Enabled == false)
            {
                _isAimed.Value = false;
                return;
            }

            _isAimed.Value = Quaternion.Angle(_transform.rotation, _targetRotation) < _preferences.MaxAngleThreshold;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _speed = 20f;
            [SerializeField] private float _maxAngleThreshold = 10f;

            public float Speed => _speed;
            public float MaxAngleThreshold => _maxAngleThreshold;
        }
    }
}