using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.SaveLoad.Core;
using Zenject;

namespace Infrastructure.Services.PersistentData
{
    public class PersistentDataService : IPersistentDataService
    {
        private ISaveLoadService _saveLoadService;

        public Data.Persistent.PersistentData Data { get; private set; }

        [Inject]
        private void Constructor(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Save() => _saveLoadService.Save(Data, nameof(Infrastructure.Data.Persistent.PersistentData));

        public void Load() => Data = _saveLoadService.Load(nameof(Data), new Data.Persistent.PersistentData());
    }
}