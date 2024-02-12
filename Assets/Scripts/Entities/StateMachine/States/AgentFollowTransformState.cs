using Entities.AI;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;

namespace Entities.StateMachine.States
{
    public class AgentFollowTransformState : IPayloadedState<Transform>, IExitable
    {
        private readonly AgentTransformFollower _transformFollower;

        public AgentFollowTransformState(AgentTransformFollower transformFollower)
        {
            _transformFollower = transformFollower;
        }

        private Transform _target;

        public void Enter(Transform target) => _transformFollower.Start(target);

        public void Exit() => _transformFollower.Stop();
    }
}