using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.SaveLoad.Core;
using Zenject;

namespace Infrastructure.Services.PersistentData
{
    public class PersistentDataService : IPersistentDataService
    {
        private const string Key = "Data";

        private ISaveLoadService _saveLoadService;

        public Data.Persistent.PersistentData Data { get; private set; }

        [Inject]
        private void Constructor(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Save() => _saveLoadService.Save(Key, Data);

        public void Load() => Data = _saveLoadService.Load(Key, new Data.Persistent.PersistentData());
    }
}