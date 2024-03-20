using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapPlayServicesState : IGameState, IState
    {
        private const bool DebugLogEnabled = true;

        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;
        private readonly ILoadingScreen _loadingScreen;

        public BootstrapPlayServicesState(IStateMachine<IGameState> stateMachine, ILogService logService, ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _loadingScreen = loadingScreen;
        }

        private bool _initialized;

        public void Enter()
        {
            _logService.Log("BootstrapPlayServicesState");
            _loadingScreen.SetInfoText("Initializing Google Play Services...");

            if (_initialized)
            {
                EnterNextState();
                return;
            }

            InitializePlayServices();
            _logService.Log("Play services initialized");
            EnterNextState();
        }

        private void InitializePlayServices()
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .RequestServerAuthCode(false)
                .Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = DebugLogEnabled;
            PlayGamesPlatform.Activate();
            _initialized = true;
        }

        private void EnterNextState() => _stateMachine.Enter<LoginState>();
    }
}