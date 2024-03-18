using System;
using GooglePlayGames.BasicApi.SavedGame;
using Infrastructure.Services.CloudSaveLoad.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.SaveLoad.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class SaveDataState : IGameState, IPayloadedState<Action>
    {
        private const string Key = "Data";

        private readonly IPersistentDataService _persistentDataService;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ILogService _logService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ICloudSaveLoadService _cloudSaveLoadService;

        public SaveDataState(IPersistentDataService persistentDataService, IStateMachine<IGameState> gameStateMachine,
            ILogService logService, ISaveLoadService saveLoadService, ICloudSaveLoadService cloudSaveLoadService)
        {
            _persistentDataService = persistentDataService;
            _gameStateMachine = gameStateMachine;
            _logService = logService;
            _saveLoadService = saveLoadService;
            _cloudSaveLoadService = cloudSaveLoadService;
        }

        public void Enter(Action onComplete = null)
        {
            _logService.Log("SaveDataState");

            SaveDataLocally();

            _logService.Log("Saved local data");

            SaveDataToCloud(onComplete);
        }

        private void EnterNextState() => _gameStateMachine.Enter<GameLoopState>();

        private void SaveDataLocally() => _saveLoadService.Save(Key, _persistentDataService.Data);

        private void SaveDataToCloud(Action onComplete)
        {
            _cloudSaveLoadService.Save(Key, _persistentDataService.Data, status =>
            {
                if (status == SavedGameRequestStatus.Success)
                    _logService.Log("Saved data to cloud");
                else
                    _logService.Log("Failed to save data to cloud");

                EnterNextState();
                onComplete?.Invoke();
            });
        }
    }
}