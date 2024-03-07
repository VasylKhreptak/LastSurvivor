using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapMessagingState : IGameState, IState
    {
        private readonly ILogService _logService;
        private IStateMachine<IGameState> _stateMachine;

        public BootstrapMessagingState(ILogService logService, IStateMachine<IGameState> stateMachine)
        {
            _logService = logService;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _logService.Log("BootstrapMessagingState");
            _stateMachine.Enter<SetupAutomaticDataSaveState>();
        }
    }
}