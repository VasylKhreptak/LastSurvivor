using System;
using GooglePlayGames.BasicApi.SavedGame;

namespace Infrastructure.Services.CloudSaveLoad.Core
{
    public interface ICloudSaveLoadService
    {
        public void Save<T>(string key, T data, Action<SavedGameRequestStatus> onComplete = null);

        public void Load<T>(string key, Action<T> onComplete);

        public void HasKey(string key, Action<bool> onComplete);

        public void Delete(string key, Action<bool> onComplete);
    }
}