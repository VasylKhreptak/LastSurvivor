using Data.Persistent;
using Infrastructure.Services.PersistentData.Core;
using Plugins.Banks;
using Zenject;

namespace Platforms.OilPlatform
{
    public class OilPlatformInstaller : MonoInstaller
    {
        private IntegerBank _bank;
        private ClampedIntegerBank _upgradeContainer;
        private OilPlatformData _platformData;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _upgradeContainer = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.UpgradeContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.OilPlatformData;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_bank).AsSingle();
            Container.BindInstance(_upgradeContainer).AsSingle();
            Container.BindInstance(_platformData).AsSingle();
        }
    }
}