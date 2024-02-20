using System;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.Transition.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class PlayState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ITransitionScreen _transitionScreen;
        private readonly IPersistentDataService _persistentDataService;

        public PlayState(IStateMachine<IGameState> stateMachine, ITransitionScreen transitionScreen,
            IPersistentDataService persistentDataService)
        {
            _stateMachine = stateMachine;
            _transitionScreen = transitionScreen;
            _persistentDataService = persistentDataService;
        }

        public void Enter() => _transitionScreen.Show(LoadAppropriateLevel);

        private void LoadAppropriateLevel()
        {
            _stateMachine.Enter<LoadNextLevelState, Action>(ClearHelicopterFuelTank);
        }

        private void ClearHelicopterFuelTank() =>
            _persistentDataService.Data.PlayerData.PlatformsData.HelicopterPlatformData.FuelTank.Clear();
    }
}