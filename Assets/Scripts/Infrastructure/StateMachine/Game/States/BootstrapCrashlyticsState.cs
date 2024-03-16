using Firebase.Crashlytics;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapCrashlyticsState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;

        public BootstrapCrashlyticsState(IStateMachine<IGameState> stateMachine, ILogService logService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("BootstrapCrashlyticsState");

            Crashlytics.IsCrashlyticsCollectionEnabled = true;
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;

            EnterNextState();
        }

        private void EnterNextState() => _stateMachine.Enter<BootstrapAdvertisementsState>();
    }
}