using System;
using Infrastructure.StateMachine.Main.States.Core;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.StateMachine.States
{
    public class AgentMoveState : IPayloadedState<AgentMoveState.Payload>, IExitable
    {
        private readonly NavMeshAgent _agent;
        private readonly Preferences _preferences;

        public AgentMoveState(NavMeshAgent agent, Preferences preferences)
        {
            _agent = agent;
            _preferences = preferences;
        }

        private Payload _payload;
        private IDisposable _destinationCheckSubscription;

        public void Enter(Payload payload)
        {
            _payload = payload;

            if (IsDestinationReached())
            {
                OnReachedDestination();
                return;
            }

            _agent.isStopped = false;

            _agent.SetDestination(payload.Position);
            StartObservingDestination();
        }

        public void Exit()
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
            Vector3.Distance(_agent.transform.position, _payload.Position) <= _preferences.StoppingDistance;

        private void OnReachedDestination() => _payload.OnComplete?.Invoke();

        public class Payload
        {
            public Vector3 Position;
            public Action OnComplete;
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