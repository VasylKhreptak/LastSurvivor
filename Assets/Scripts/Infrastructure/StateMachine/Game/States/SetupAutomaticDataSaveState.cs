using Infrastructure.Data.SaveLoad;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class SetupAutomaticDataSaveState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;
        private readonly ApplicationPauseDataSaver _automaticDataSaver;
        private readonly ILoadingScreen _loadingScreen;

        public SetupAutomaticDataSaveState(IStateMachine<IGameState> stateMachine, ILogService logService,
            DisposableManager disposableManager, ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _loadingScreen = loadingScreen;
            _automaticDataSaver = new ApplicationPauseDataSaver(_stateMachine);
            disposableManager.Add(_automaticDataSaver);
        }

        private bool _initialized;

        public void Enter()
        {
            _logService.Log("SetupAutomaticDataSaveState");
            _loadingScreen.SetInfoText("Setting up automatic data save...");

            if (_initialized == false)
            {
                _automaticDataSaver.Initialize();
                _initialized = true;
            }

            EnterNextState();
        }

        private void EnterNextState() => _stateMachine.Enter<ScheduleNotificationsState>();
    }
}