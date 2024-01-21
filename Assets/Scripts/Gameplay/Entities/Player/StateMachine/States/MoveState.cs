using Entities.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine.AI;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class MoveState : AgentMoveState<IPlayerState>, IPlayerState
    {
        public MoveState(NavMeshAgent agent, Preferences preferences, IStateMachine<IPlayerState> stateMachine)
            : base(agent, preferences, stateMachine) { }
    }
}