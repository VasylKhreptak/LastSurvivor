using Entities.AI;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.StateMachine.States
{
    public class AgentFollowTransformState : IPayloadedState<Transform>, IExitable
    {
        private readonly NavMeshAgent _agent;
        private readonly AgentTransformFollower.Preferences _preferences;
        private readonly AgentTransformFollower _agentTransformFollower;

        public AgentFollowTransformState(NavMeshAgent agent, AgentTransformFollower.Preferences preferences)
        {
            _agent = agent;
            _preferences = preferences;
            _agentTransformFollower = new AgentTransformFollower(agent, preferences);
        }

        private Transform _target;

        public void Enter(Transform target) => _agentTransformFollower.Follow(target);

        public void Exit() => _agentTransformFollower.Stop();
    }
}