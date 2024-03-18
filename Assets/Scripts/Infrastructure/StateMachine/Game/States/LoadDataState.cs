using System;
using Infrastructure.Data.Persistent;
using Infrastructure.Services.CloudSaveLoad.Core;
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

        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ILogService _logService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ICloudSaveLoadService _cloudSaveLoadService;

        public LoadDataState(IStateMachine<IGameState> gameStateMachine, IPersistentDataService persistentDataService,
            ILogService logService, ISaveLoadService saveLoadService, ICloudSaveLoadService cloudSaveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
            _logService = logService;
            _saveLoadService = saveLoadService;
            _cloudSaveLoadService = cloudSaveLoadService;
        }

        public void Enter()
        {
            _logService.Log("LoadDataState");

            LoadData(EnterNextState);
        }

        private void LoadData(Action onComplete)
        {
            _cloudSaveLoadService.Load<PersistentData>(Key, data =>
            {
                if (data == null)
                {
                    _persistentDataService.Data = _saveLoadService.Load(Key, new PersistentData());
                    _logService.Log("Loaded local data");
                    onComplete();
                    return;
                }

                _persistentDataService.Data = data;
                _logService.Log("Loaded cloud data");
                onComplete();
            });
        }

        private void EnterNextState() => _gameStateMachine.Enter<ApplySavedSettingsState>();
    }
}