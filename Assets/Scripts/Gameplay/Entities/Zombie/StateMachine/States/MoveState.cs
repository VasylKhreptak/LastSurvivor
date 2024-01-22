using Entities.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using UnityEngine.AI;

namespace Gameplay.Entities.Zombie.StateMachine.States
{
    public class MoveState : AgentMoveState, IZombieState
    {
        public MoveState(NavMeshAgent agent, Preferences preferences)
            : base(agent, preferences) { }
    }
}