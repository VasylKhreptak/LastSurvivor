using Analytics;
using Firebase.Analytics;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class SaveDataState : IGameState, IState
    {
        private readonly IPersistentDataService _persistentDataService;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ILogService _logService;

        public SaveDataState(IPersistentDataService persistentDataService,
            IStateMachine<IGameState> gameStateMachine, ILogService logService)
        {
            _persistentDataService = persistentDataService;
            _gameStateMachine = gameStateMachine;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("SaveDataState");
            _persistentDataService.Save();
            FirebaseAnalytics.LogEvent(AnalyticEvents.SavedData);
            _logService.Log("Saved data");
            _gameStateMachine.Back();
        }
    }
}