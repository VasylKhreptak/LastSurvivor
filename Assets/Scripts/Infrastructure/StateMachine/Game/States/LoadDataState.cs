using Infrastructure.Services.SaveLoadHandler.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadDataState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ISaveLoadHandlerService _saveLoadHandlerService;

        public LoadDataState(IStateMachine<IGameState> gameStateMachine, ISaveLoadHandlerService saveLoadHandlerService)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadHandlerService = saveLoadHandlerService;
        }

        public void Enter()
        {
            _saveLoadHandlerService.Load();
            _gameStateMachine.Enter<BootstrapAnalyticsState>();
        }
    }
}