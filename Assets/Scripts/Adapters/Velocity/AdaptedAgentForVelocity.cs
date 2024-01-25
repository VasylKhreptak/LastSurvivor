using Adapters.Velocity.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Adapters.Velocity
{
    public class AdaptedAgentForVelocity : IVelocityAdapter
    {
        private readonly NavMeshAgent _agent;

        public AdaptedAgentForVelocity(NavMeshAgent agent)
        {
            _agent = agent;
        }

        public Vector3 Value => _agent.velocity;
    }
}