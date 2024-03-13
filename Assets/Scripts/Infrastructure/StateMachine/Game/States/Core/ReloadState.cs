using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.Transition.Core;

namespace Infrastructure.StateMachine.Game.States.Core
{
    public class ReloadState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ITransitionScreen _transitionScreen;
        private readonly ILogService _logService;

        public ReloadState(IStateMachine<IGameState> stateMachine, ITransitionScreen transitionScreen, ILogService logService)
        {
            _stateMachine = stateMachine;
            _transitionScreen = transitionScreen;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("ReloadState");

            _transitionScreen.Show(() =>
            {
                _stateMachine.Enter<SaveDataState>();
                _stateMachine.Enter<BootstrapState>();
                _transitionScreen.HideImmediately();
            });
        }
    }
}