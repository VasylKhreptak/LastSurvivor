using System;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.StateMachine.States
{
    public class AgentFollowTransformState : IPayloadedState<Transform>, IExitable
    {
        private readonly NavMeshAgent _agent;
        private readonly Preferences _preferences;

        public AgentFollowTransformState(NavMeshAgent agent, Preferences preferences)
        {
            _agent = agent;
            _preferences = preferences;
        }

        private Transform _target;

        public void Enter(Transform target)
        {
            if (_agent.isActiveAndEnabled == false)
                return;

            _target = target;
            
            _agent.isStopped = false;
            
            ///
        }

        public void Exit()
        {
            if (_agent.isActiveAndEnabled)
                _agent.isStopped = true;
            
            ///
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