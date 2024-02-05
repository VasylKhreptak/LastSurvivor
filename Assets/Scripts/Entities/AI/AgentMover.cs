using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.AI
{
    public class AgentMover
    {
        private readonly NavMeshAgent _agent;
        private readonly Preferences _preferences;

        public AgentMover(NavMeshAgent agent, Preferences preferences)
        {
            _agent = agent;
            _preferences = preferences;
        }

        private IDisposable _destinationCheckSubscription;

        private Vector3 _targetPosition;
        private Action _onComplete;

        public void SetDestination(Vector3 position, Action onComplete)
        {
            Stop();
            
            _targetPosition = position;
            _onComplete = onComplete;

            if (IsDestinationReached())
            {
                OnReachedDestination();
                return;
            }

            _agent.isStopped = false;

            _agent.SetDestination(_targetPosition);
            StartObservingDestination();
        }

        public void Stop()
        {
            StopObservingDestination();

            if (_agent.isActiveAndEnabled)
                _agent.isStopped = true;
        }

        private void StartObservingDestination()
        {
            _destinationCheckSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.DestinationCheckInterval))
                .Subscribe(_ => CheckDestination());
        }

        private void StopObservingDestination() => _destinationCheckSubscription?.Dispose();

        private void CheckDestination()
        {
            if (IsDestinationReached())
                OnReachedDestination();
        }

        private bool IsDestinationReached() =>
            Vector3.Distance(_agent.transform.position, _targetPosition) <= _preferences.StoppingDistance;

        private void OnReachedDestination() => _onComplete?.Invoke();

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _stoppingDistance = 0.3f;
            [SerializeField] private float _destinationCheckInterval = 0.1f;

            public float StoppingDistance => _stoppingDistance;
            public float DestinationCheckInterval => _destinationCheckInterval;
        }
    }
}