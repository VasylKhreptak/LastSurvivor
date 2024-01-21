using Entities.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine.AI;

namespace Gameplay.Entities.Zombie.StateMachine.States
{
    public class MoveState : AgentMoveState, IZombieState
    {
        public MoveState(NavMeshAgent agent, Preferences preferences, IStateMachine<IPlayerState> stateMachine)
            : base(agent, preferences, stateMachine) { }
    }
}