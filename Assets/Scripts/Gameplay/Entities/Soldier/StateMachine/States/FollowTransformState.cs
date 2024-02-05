using Entities.AI;
using Entities.StateMachine.States;
using Gameplay.Entities.Soldier.StateMachine.States.Core;

namespace Gameplay.Entities.Soldier.StateMachine.States
{
    public class FollowTransformState : AgentFollowTransformState, ISoldierState
    {
        public FollowTransformState(AgentTransformFollower transformFollower) : base(transformFollower) { }
    }
}