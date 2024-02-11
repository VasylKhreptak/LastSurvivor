using System;
using Pathfinding;
using UniRx;
using UnityEngine;

namespace Entities.AI
{
    public class AgentMover
    {
        private readonly IAstarAI _ai;
        private readonly Preferences _preferences;

        public AgentMover(IAstarAI ai, Preferences preferences)
        {
            _ai = ai;
            _preferences = preferences;
        }

        private IDisposable _destinationCheckSubscription;

        private Vector3 _destination;
        private Action _onComplete;

        public void SetDestination(Vector3 position, Action onComplete = null)
        {
            Stop();

            _destination = position;
            _onComplete = onComplete;

            _ai.isStopped = false;
            _ai.destination = _destination;
            StartObservingDestination();
        }

        public void Stop()
        {
            StopObservingDestination();
            _ai.isStopped = true;
        }

        private void StartObservingDestination()
        {
            _destinationCheckSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.DestinationCheckInterval))
                .DelayFrame(1)
                .Subscribe(_ => CheckDestination());
        }

        private void StopObservingDestination() => _destinationCheckSubscription?.Dispose();

        private void CheckDestination()
        {
            if (IsDestinationReached())
                OnReachedDestination();
        }

        private bool IsDestinationReached() => _ai.reachedEndOfPath;

        private void OnReachedDestination()
        {
            Stop();
            _onComplete?.Invoke();
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _destinationCheckInterval = 0.1f;

            public float DestinationCheckInterval => _destinationCheckInterval;
        }
    }
}