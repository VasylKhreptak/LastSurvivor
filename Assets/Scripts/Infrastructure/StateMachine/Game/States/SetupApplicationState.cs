using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Settings;

namespace Infrastructure.StateMachine.Game.States
{
    public class SetupApplicationState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly SettingsApplier _settingsApplier;
        private readonly IPersistentDataService _persistentDataService;

        public SetupApplicationState(IStateMachine<IGameState> gameStateMachine, SettingsApplier settingsApplier,
            IPersistentDataService persistentDataService)
        {
            _gameStateMachine = gameStateMachine;
            _settingsApplier = settingsApplier;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            ApplySettings();
            _gameStateMachine.Enter<BootstrapAnalyticsState>();
        }

        private void ApplySettings() => _settingsApplier.Apply(_persistentDataService.Data.Settings);
    }
}