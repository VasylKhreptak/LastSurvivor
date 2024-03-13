using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States.Core
{
    public class ReloadState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;

        public ReloadState(IStateMachine<IGameState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _stateMachine.Enter<SaveDataState>();
            _stateMachine.Enter<BootstrapState>();
        }
    }
}