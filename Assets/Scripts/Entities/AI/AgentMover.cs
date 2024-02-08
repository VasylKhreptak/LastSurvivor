using System;
using System.Linq;
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

        private Vector3 _destination;
        private bool _maxPossibleDestination;
        private Action _onComplete;

        public void SetDestination(Vector3 position, bool maxPossibleDestination = false, Action onComplete = null)
        {
            Stop();

            _destination = position;
            _maxPossibleDestination = maxPossibleDestination;
            _onComplete = onComplete;

            if (IsDestinationReached())
            {
                OnReachedDestination();
                return;
            }

            _agent.isStopped = false;

            _agent.SetDestination(_destination);
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
                .Subscribe(_ =>
                {
                    if (_maxPossibleDestination && _agent.hasPath)
                        _destination = _agent.path.corners.Last();

                    CheckDestination();
                });
        }

        private void StopObservingDestination() => _destinationCheckSubscription?.Dispose();

        private void CheckDestination()
        {
            if (IsDestinationReached())
                OnReachedDestination();
        }

        private bool IsDestinationReached() =>
            Vector3.Distance(_agent.transform.position, _destination) <= _preferences.StoppingDistance;

        private void OnReachedDestination()
        {
            Stop();
            _onComplete?.Invoke();
        }

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