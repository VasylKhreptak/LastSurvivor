using Infrastructure.Services.Log.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Settings;

namespace Infrastructure.StateMachine.Game.States
{
    public class ApplySavedSettingsState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly SettingsApplier _settingsApplier;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ILogService _logService;

        public ApplySavedSettingsState(IStateMachine<IGameState> gameStateMachine, SettingsApplier settingsApplier,
            IPersistentDataService persistentDataService, ILogService logService)
        {
            _gameStateMachine = gameStateMachine;
            _settingsApplier = settingsApplier;
            _persistentDataService = persistentDataService;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("ApplySavedSettingsState");
            ApplySettings();
            EnterNextState();
        }

        private void ApplySettings() => _settingsApplier.Apply(_persistentDataService.Data.Settings);

        private void EnterNextState() => _gameStateMachine.Enter<BootstrapFirebaseState>();
    }
}