using Data.Persistent;
using Data.Static.Balance;
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
        [SerializeField] private ReceiveZone _receiveZone;

        private IntegerBank _bank;
        private ClampedIntegerBank _upgradeContainer;
        private HelicopterPlatformData _platformData;
        private GamePrefabs _prefabs;
        private HelicopterPlatformUpgradePreferences _helicopterPlatformUpgradePreferences;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _upgradeContainer = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.UpgradeContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData;
            _prefabs = staticDataService.Prefabs;
            _helicopterPlatformUpgradePreferences = staticDataService.Balance.HelicopterPlatformUpgradePreferences;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _oilBarrelReceiver ??= GetComponentInChildren<OilBarrelReceiver>(true);
            _receiveZone ??= GetComponentInChildren<ReceiveZone>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_bank).AsSingle();
            Container.BindInstance(_upgradeContainer).AsSingle();
            Container.BindInstance(_platformData).AsSingle();
            Container.BindInstance(_oilBarrelReceiver).AsSingle();
            Container.BindInstance(_helicopterPlatformUpgradePreferences).AsSingle();

            BindUpgradeLogic();
        }

        private void BindUpgradeLogic()
        {
            Container.BindInstance(_prefabs.Gear).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_receiveZone).AsSingle();
            Container.BindInterfacesTo<HelicopterUpgrader>().AsSingle();
        }
    }
}