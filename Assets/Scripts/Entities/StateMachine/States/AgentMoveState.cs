using System;
using Entities.AI;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.StateMachine.States
{
    public class AgentMoveState : IPayloadedState<AgentMoveState.Payload>, IExitable
    {
        private readonly NavMeshAgent _agent;
        private readonly AgentMover.Preferences _preferences;
        private readonly AgentMover _agentMover;

        public AgentMoveState(NavMeshAgent agent, AgentMover.Preferences preferences)
        {
            _agent = agent;
            _preferences = preferences;
            _agentMover = new AgentMover(agent, preferences);
        }

        public void Enter(Payload payload) => _agentMover.SetDestination(payload.Position, payload.OnComplete);

        public void Exit() => _agentMover.Stop();

        public class Payload
        {
            public Vector3 Position;
            public Action OnComplete;
        }
    }
}