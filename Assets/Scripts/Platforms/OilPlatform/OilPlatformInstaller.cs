using Infrastructure.Services.PersistentData.Core;
using Plugins.Banks;
using Zenject;

namespace Platforms.OilPlatform
{
    public class OilPlatformInstaller : MonoInstaller
    {
        private IntegerBank _bank;
        private ClampedIntegerBank _upgradeContainer;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _upgradeContainer = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.UpgradeContainer;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_bank).AsSingle();
            Container.BindInstance(_upgradeContainer).AsSingle();
        }
    }
}