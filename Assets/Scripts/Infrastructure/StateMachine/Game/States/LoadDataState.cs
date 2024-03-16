using Infrastructure.Data.Persistent;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.SaveLoad.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadDataState : IState, IGameState
    {
        private const string Key = "Data";
        private const int MaxSavedToDisplay = 5;
        private const bool AllowCreateNew = false;
        private const bool AllowDelete = true;

        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ILogService _logService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadDataState(IStateMachine<IGameState> gameStateMachine, IPersistentDataService persistentDataService,
            ILogService logService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
            _logService = logService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            _logService.Log("LoadDataState");

            LoadLocalData();

            _gameStateMachine.Enter<ApplySavedSettingsState>();
        }

        private void LoadLocalData() => _persistentDataService.Data = _saveLoadService.Load(Key, new PersistentData());
    }
}