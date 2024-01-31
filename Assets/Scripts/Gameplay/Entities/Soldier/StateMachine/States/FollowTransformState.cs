using Entities.StateMachine.States;
using Gameplay.Entities.Soldier.StateMachine.States.Core;
using UnityEngine.AI;

namespace Gameplay.Entities.Soldier.StateMachine.States
{
    public class FollowTransformState : AgentFollowTransformState, ISoldierState
    {
        public FollowTransformState(NavMeshAgent agent, Preferences preferences) : base(agent, preferences) { }
    }
}