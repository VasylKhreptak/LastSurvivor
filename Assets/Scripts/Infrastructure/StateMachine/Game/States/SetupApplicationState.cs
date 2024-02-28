using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Settings;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace Infrastructure.StateMachine.Game.States
{
    public class SetupApplicationState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly SettingsApplier _settingsApplier;

        public SetupApplicationState(IStateMachine<IGameState> gameStateMachine, SettingsApplier settingsApplier)
        {
            _gameStateMachine = gameStateMachine;
            _settingsApplier = settingsApplier;
        }

        public void Enter()
        {
            DisableSleepTimeout();
            ApplySettings();
            _gameStateMachine.Enter<BootstrapAnalyticsState>();
        }

        private void DisableSleepTimeout() => Screen.sleepTimeout = SleepTimeout.NeverSleep;

        private void ApplySettings() => _settingsApplier.Apply();
    }
}