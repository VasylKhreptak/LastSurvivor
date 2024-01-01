using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapAnalyticsState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IPersistentDataService _persistentDataService;

        public BootstrapAnalyticsState(IStateMachine<IGameState> gameStateMachine, IPersistentDataService persistentDataService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            _persistentDataService.PersistentData.AnalyticsData.SessionsCount++;
            _gameStateMachine.Enter<FinalizeBootstrapState>();
        }
    }
}