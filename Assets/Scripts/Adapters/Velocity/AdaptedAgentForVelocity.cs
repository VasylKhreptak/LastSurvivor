using Adapters.Velocity.Core;
using Pathfinding;
using UnityEngine;

namespace Adapters.Velocity
{
    public class AdaptedAgentForVelocity : IVelocityAdapter
    {
        private readonly IAstarAI _ai;

        public AdaptedAgentForVelocity(IAstarAI ai)
        {
            _ai = ai;
        }

        public Vector3 Value => _ai.velocity;
    }
}