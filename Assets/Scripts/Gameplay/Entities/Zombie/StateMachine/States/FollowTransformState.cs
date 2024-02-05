using Entities.AI;
using Entities.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;

namespace Gameplay.Entities.Zombie.StateMachine.States
{
    public class FollowTransformState : AgentFollowTransformState, IZombieState
    {
        public FollowTransformState(AgentTransformFollower transformFollower) : base(transformFollower) { }
    }
}