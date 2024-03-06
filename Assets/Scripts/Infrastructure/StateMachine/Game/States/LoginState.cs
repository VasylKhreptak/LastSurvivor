using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoginState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;

        public LoginState(IStateMachine<IGameState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            //login

            _stateMachine.Enter<LoadDataState>();
        }
    }
}