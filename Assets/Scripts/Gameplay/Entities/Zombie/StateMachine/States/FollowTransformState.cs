using Entities.AI;
using Entities.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using UnityEngine.AI;

namespace Gameplay.Entities.Zombie.StateMachine.States
{
    public class FollowTransformState : AgentFollowTransformState, IZombieState
    {
        public FollowTransformState(NavMeshAgent agent, AgentTransformFollower.Preferences preferences)
            : base(agent, preferences) { }
    }
}