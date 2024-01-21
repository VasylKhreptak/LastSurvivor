using Entities.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine.AI;

namespace Gameplay.Entities.Zombie.StateMachine.States
{
    public class MoveState : AgentMoveState<IZombieState>, IZombieState
    {
        public MoveState(NavMeshAgent agent, Preferences preferences, IStateMachine<IZombieState> stateMachine)
            : base(agent, preferences, stateMachine) { }
    }
}