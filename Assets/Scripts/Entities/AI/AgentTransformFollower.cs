using System;
using UniRx;
using UnityEngine;

namespace Entities.AI
{
    public class AgentTransformFollower
    {
        private readonly AgentMover _agentMover;
        private readonly Preferences _preferences;

        public AgentTransformFollower(AgentMover agentMover, Preferences preferences)
        {
            _agentMover = agentMover;
            _preferences = preferences;
        }

        private IDisposable _destinationUpdateSubscription;

        private Transform _target;

        private Vector3 _targetPosition;
        private Vector3 _lastDestinationPosition;

        public void Follow(Transform target)
        {
            Stop();

            _target = target;
            _lastDestinationPosition = _target.position;

            _agentMover.SetDestination(_lastDestinationPosition);
            StartUpdatingDestination();
        }

        public void Stop()
        {
            StopUpdatingDestination();
            _agentMover.Stop();
        }

        private void StartUpdatingDestination()
        {
            _destinationUpdateSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.UpdateInterval))
                .Subscribe(_ => TryUpdateDestination());
        }

        private void StopUpdatingDestination() => _destinationUpdateSubscription?.Dispose();

        private void TryUpdateDestination()
        {
            _targetPosition = _target.position;

            if (Vector3.Distance(_targetPosition, _lastDestinationPosition) > _preferences.PositionThreshold)
            {
                _agentMover.SetDestination(_targetPosition);
                _lastDestinationPosition = _targetPosition;
            }
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _positionThreshold = 0.1f;
            [SerializeField] private float _updateInterval = 0.1f;

            public float PositionThreshold => _positionThreshold;
            public float UpdateInterval => _updateInterval;
        }
    }
}