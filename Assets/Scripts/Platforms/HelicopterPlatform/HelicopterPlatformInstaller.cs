using Data.Persistent;
using Infrastructure.Data.Static;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Plugins.Banks;
using UnityEngine;
using Zenject;

namespace Platforms.HelicopterPlatform
{
    public class HelicopterPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private OilBarrelReceiver _oilBarrelReceiver;

        private IntegerBank _bank;
        private ClampedIntegerBank _upgradeContainer;
        private HelicopterPlatformData _platformData;
        private GamePrefabs _prefabs;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _upgradeContainer = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.UpgradeContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData;
            _prefabs = staticDataService.Prefabs;
        }

        #region MonoBehaviour

        private void OnValidate() => _oilBarrelReceiver ??= GetComponentInChildren<OilBarrelReceiver>(true);

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_bank).AsSingle();
            Container.BindInstance(_upgradeContainer).AsSingle();
            Container.BindInstance(_platformData).AsSingle();
            Container.BindInstance(_platformData.HelicopterData).AsSingle();
            Container.BindInstance(_oilBarrelReceiver).AsSingle();

            BindUpgradeZone();
        }

        private void BindUpgradeZone()
        {
            Container.BindInstance(_prefabs.Gear).WhenInjectedInto<HelicopterUpgradeZone>();
        }
    }
}