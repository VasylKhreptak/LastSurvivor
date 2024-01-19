using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main;

namespace Gameplay.Entities.Player.StateMachine
{
    public class PlayerStateMachine : StateMachine<IPlayerState>
    {
        protected PlayerStateMachine(PlayerStateFactory stateFactory) : base(stateFactory) { }
    }
}