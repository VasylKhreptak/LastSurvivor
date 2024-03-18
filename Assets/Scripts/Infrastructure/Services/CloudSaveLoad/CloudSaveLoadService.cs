using System;
using System.Text;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Infrastructure.Services.CloudSaveLoad.Core;
using Infrastructure.Services.Log.Core;
using Newtonsoft.Json;

namespace Infrastructure.Services.CloudSaveLoad
{
    public class CloudSaveLoadService : ICloudSaveLoadService
    {
        private const DataSource Source = DataSource.ReadCacheOrNetwork;
        private const ConflictResolutionStrategy Strategy = ConflictResolutionStrategy.UseLastKnownGood;

        private readonly ILogService _logService;

        public CloudSaveLoadService(ILogService logService)
        {
            _logService = logService;
        }

        public void Save<T>(string key, T data, Action<SavedGameRequestStatus> onComplete)
        {
            _logService.Log("Cloud save request");

            OpenSavedGame(key, (status, metadata) =>
            {
                _logService.Log("Open saved game request status: " + status);

                if (status != SavedGameRequestStatus.Success)
                {
                    onComplete(status);
                    return;
                }

                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder()
                    .WithUpdatedDescription("Saved game at " + DateTime.Now);

                string jsonData = JsonConvert.SerializeObject(data);
                byte[] binaryData = Encoding.UTF8.GetBytes(jsonData);

                SavedGameMetadataUpdate updatedMetadataUpdate = builder.Build();
                savedGameClient.CommitUpdate(metadata, updatedMetadataUpdate, binaryData, (status, _) =>
                {
                    _logService.Log("Commit update request status: " + status);
                    onComplete(status);
                });
            });
        }

        public void Load<T>(string key, Action<T> onComplete)
        {
            _logService.Log("Cloud load request");

            OpenSavedGame(key, (status, metadata) =>
            {
                _logService.Log("Open saved game request status: " + status);

                if (status != SavedGameRequestStatus.Success)
                {
                    onComplete(default);
                    return;
                }

                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.ReadBinaryData(metadata, (status, data) =>
                {
                    _logService.Log("Read binary data request status: " + status);

                    if (status != SavedGameRequestStatus.Success)
                    {
                        onComplete(default);
                        return;
                    }

                    string jsonData = Encoding.UTF8.GetString(data);
                    T result = JsonConvert.DeserializeObject<T>(jsonData);
                    onComplete(result);
                });
            });
        }

        public void HasKey(string key, Action<bool> onComplete) =>
            OpenSavedGame(key, (status, _) => onComplete(status == SavedGameRequestStatus.Success));

        public void Delete(string key, Action<bool> onComplete)
        {
            OpenSavedGame(key, (status, metadata) =>
            {
                if (status != SavedGameRequestStatus.Success)
                {
                    onComplete(false);
                    return;
                }

                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.Delete(metadata);
                onComplete(true);
            });
        }

        private void OpenSavedGame(string key, Action<SavedGameRequestStatus, ISavedGameMetadata> onComplete)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            if (savedGameClient == null)
            {
                onComplete(SavedGameRequestStatus.InternalError, null);
                return;
            }

            savedGameClient.OpenWithAutomaticConflictResolution(key, Source, Strategy, onComplete);
        }
    }
}