using System;
using Infrastructure.Services.Log.Core;
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
        private readonly ILogService _logService;

        public PlayState(IStateMachine<IGameState> stateMachine, ITransitionScreen transitionScreen,
            IPersistentDataService persistentDataService, ILogService logService)
        {
            _stateMachine = stateMachine;
            _transitionScreen = transitionScreen;
            _persistentDataService = persistentDataService;
            _logService = logService;
        }

        public event Action OnEnter;

        public void Enter()
        {
            _logService.Log("PlayState");
            _transitionScreen.Show(LoadAppropriateLevel);
        }

        private void LoadAppropriateLevel()
        {
            _stateMachine.Enter<LoadLevelState, Action>(ClearHelicopterFuelTank);
            OnEnter?.Invoke();
        }

        private void ClearHelicopterFuelTank() =>
            _persistentDataService.Data.PlayerData.PlatformsData.HelicopterPlatformData.FuelTank.Clear();
    }
}