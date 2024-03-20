using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.SaveLoad.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Newtonsoft.Json;

namespace Infrastructure.StateMachine.Game.States
{
    public class SaveDataState : IGameState, IPayloadedState<Action>
    {
        private const string Key = "Data";

        private readonly IPersistentDataService _persistentDataService;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ILogService _logService;
        private readonly ISaveLoadService _saveLoadService;

        public SaveDataState(IPersistentDataService persistentDataService, IStateMachine<IGameState> gameStateMachine,
            ILogService logService, ISaveLoadService saveLoadService)
        {
            _persistentDataService = persistentDataService;
            _gameStateMachine = gameStateMachine;
            _logService = logService;
            _saveLoadService = saveLoadService;
        }

        public async void Enter(Action onComplete = null)
        {
            _logService.Log("SaveDataState");

            UpdateSaveDateStamp();

            SaveDataLocally();

            _logService.Log("Saved local data");

            await SaveDataToDb();

            EnterNextState();

            onComplete?.Invoke();
        }

        private void UpdateSaveDateStamp() => _persistentDataService.Data.Metadata.SaveDateStamp = DateTime.UtcNow;

        private void EnterNextState() => _gameStateMachine.Enter<GameLoopState>();

        private void SaveDataLocally() => _saveLoadService.Save(Key, _persistentDataService.Data);

        private async Task SaveDataToDb()
        {
            FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

            if (user == null)
            {
                _logService.Log("User is null. Data not saved to db");
                return;
            }

            DatabaseReference dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            string json = JsonConvert.SerializeObject(_persistentDataService.Data);

            await dbReference.Child("Users").Child(user.UserId).Child(Key).SetRawJsonValueAsync(json);

            _logService.Log("Saved data to db");
        }
    }
}