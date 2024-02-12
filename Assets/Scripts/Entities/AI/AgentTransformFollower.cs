using System;
using Pathfinding;
using UniRx;
using UnityEngine;

namespace Entities.AI
{
    public class AgentTransformFollower
    {
        private readonly IAstarAI _ai;
        private readonly Preferences _preferences;

        public AgentTransformFollower(IAstarAI ai, Preferences preferences)
        {
            _ai = ai;
            _preferences = preferences;
        }

        private Transform _target;

        private IDisposable _updateSubscription;

        public void Start(Transform target)
        {
            Stop();

            _target = target;

            _ai.isStopped = false;
            StartUpdatingDestination();
        }

        public void Stop()
        {
            StopUpdatingDestination();
            _ai.isStopped = true;
        }

        private void StartUpdatingDestination()
        {
            _updateSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.UpdateInterval))
                .Subscribe(_ => UpdateDestination());
        }

        private void StopUpdatingDestination() => _updateSubscription?.Dispose();

        private void UpdateDestination() => _ai.destination = _target.position;

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _updateInterval = 0.1f;

            public float UpdateInterval => _updateInterval;
        }
    }
}