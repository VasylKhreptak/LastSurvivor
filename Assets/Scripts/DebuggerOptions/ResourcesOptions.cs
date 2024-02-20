using System.ComponentModel;
using Infrastructure.Services.PersistentData.Core;

namespace DebuggerOptions
{
    public class ResourcesOptions
    {
        private readonly IPersistentDataService _persistentDataService;

        public ResourcesOptions(IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        [Category("Balance"), SROptions.Increment(100)]
        public int Money
        {
            get => _persistentDataService.Data.PlayerData.Resources.Money.Value.Value;
            set => _persistentDataService.Data.PlayerData.Resources.Money.SetValue(value);
        }
        
        [Category("Balance"), SROptions.Increment(100)]
        public int Gears
        {
            get => _persistentDataService.Data.PlayerData.Resources.Gears.Value.Value;
            set => _persistentDataService.Data.PlayerData.Resources.Gears.SetValue(value);
        }
    }
}