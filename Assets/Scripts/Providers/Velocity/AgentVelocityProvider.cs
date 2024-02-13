using Pathfinding;
using Providers.Velocity.Core;
using UnityEngine;

namespace Providers.Velocity
{
    public class AgentVelocityProvider : IVelocityProvider
    {
        private readonly IAstarAI _ai;

        public AgentVelocityProvider(IAstarAI ai) => _ai = ai;

        public Vector3 Value => _ai.velocity;
    }
}