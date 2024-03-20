using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Infrastructure.Data.Persistent;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.SaveLoad.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Newtonsoft.Json;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadDataState : IState, IGameState
    {
        private const string Key = "Data";

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

        public async void Enter()
        {
            _logService.Log("LoadDataState");

            await LoadData();
            EnterNextState();
        }

        private async Task LoadData()
        {
            FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

            if (user == null)
            {
                LoadLocalData();
                return;
            }

            PersistentData dbData = await GetDbData();

            if (dbData == null)
            {
                LoadLocalData();
                return;
            }

            PersistentData localData = GetLocalData();

            if (localData.Metadata.SaveDateStamp > dbData.Metadata.SaveDateStamp)
            {
                _persistentDataService.Data = localData;
                _logService.Log("Loaded local data. Db data is outdated");
                return;
            }

            _persistentDataService.Data = dbData;
            _logService.Log("Loaded db data");
        }

        private void LoadLocalData()
        {
            _persistentDataService.Data = GetLocalData();
            _logService.Log("Loaded local data");
        }

        private PersistentData GetLocalData() => _saveLoadService.Load(Key, new PersistentData());

        private async Task<PersistentData> GetDbData()
        {
            FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

            if (user == null)
                return null;

            DatabaseReference dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            DataSnapshot snapshot = await dbReference.Child("Users").Child(user.UserId).Child(Key).GetValueAsync();

            string json = snapshot.GetRawJsonValue();

            PersistentData data = null;

            try
            {
                data = JsonConvert.DeserializeObject<PersistentData>(json);
            }
            catch (Exception)
            {
                // ignored
            }

            return data;
        }

        private void EnterNextState() => _gameStateMachine.Enter<ApplySavedSettingsState>();
    }
}