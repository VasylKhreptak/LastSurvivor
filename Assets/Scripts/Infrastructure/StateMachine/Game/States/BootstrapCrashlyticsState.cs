using Firebase.Crashlytics;
using Infrastructure.LoadingScreen.Core;
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
        private readonly ILoadingScreen _loadingScreen;

        public BootstrapCrashlyticsState(IStateMachine<IGameState> stateMachine, ILogService logService, ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _loadingScreen = loadingScreen;
        }

        public void Enter()
        {
            _logService.Log("BootstrapCrashlyticsState");
            _loadingScreen.SetInfoText("Initializing crashlytics...");

            Crashlytics.IsCrashlyticsCollectionEnabled = true;
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;

            EnterNextState();
        }

        private void EnterNextState() => _stateMachine.Enter<BootstrapAdvertisementsState>();
    }
}