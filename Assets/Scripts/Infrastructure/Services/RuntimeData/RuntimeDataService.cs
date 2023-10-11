using Infrastructure.Services.RuntimeData.Core;
using Infrastructure.Services.SaveLoad.Core;
using Zenject;

namespace Infrastructure.Services.RuntimeData
{
    public class RuntimeDataService : IRuntimeDataService
    {
        private ISaveLoadService _saveLoadService;

        public Data.Runtime.RuntimeData RuntimeData { get; set; }

        [Inject]
        private void Constructor(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Save()
        {
            _saveLoadService.Save(RuntimeData, nameof(Data.Runtime.RuntimeData));
        }

        public void Load()
        {
            RuntimeData = _saveLoadService.Load(nameof(RuntimeData), new Data.Runtime.RuntimeData());
        }
    }
}
