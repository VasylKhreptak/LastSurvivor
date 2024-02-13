using System;
using Entities.AI;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;

namespace Entities.StateMachine.States
{
    public class AgentMoveState : IPayloadedState<AgentMoveState.Payload>, IExitable
    {
        private readonly AgentMover _agentMover;

        public AgentMoveState(AgentMover agentMover)
        {
            _agentMover = agentMover;
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