using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.SaveLoad.Core;
using Zenject;

namespace Infrastructure.Services.PersistentData
{
    public class PersistentDataService : IPersistentDataService
    {
        private ISaveLoadService _saveLoadService;

        public Data.Persistent.PersistentData PersistentData { get; set; }

        [Inject]
        private void Constructor(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Save()
        {
            _saveLoadService.Save(PersistentData, nameof(Data.Persistent.PersistentData));
        }

        public void Load()
        {
            PersistentData = _saveLoadService.Load(nameof(PersistentData), new Data.Persistent.PersistentData());
        }
    }
}