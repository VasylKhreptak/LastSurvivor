using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using Flexalon;
using Grid;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Platforms.Zones;
using Plugins.Banks;
using UI.ClampedBanks;
using UnityEngine;
using Zenject;

namespace Platforms.OilPlatform
{
    public class OilPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private FlexalonGridLayout _grid;
        [SerializeField] private ReceiveZone _receiveZone;
        [SerializeField] private FuelSpawner _fuelSpawner;

        private IntegerBank _bank;
        private ClampedIntegerBank _upgradeContainer;
        private OilPlatformData _platformData;
        private OilPlatformPreferences _preferences;
        private GamePrefabs _prefabs;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _upgradeContainer = persistentDataService.PersistentData.PlayerData.PlatformsData.OilPlatformData.UpgradeContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.PlatformsData.OilPlatformData;
            _preferences = staticDataService.Balance.OilPlatformPreferences;
            _prefabs = staticDataService.Prefabs;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _grid ??= GetComponentInChildren<FlexalonGridLayout>(true);
            _receiveZone ??= GetComponentInChildren<ReceiveZone>(true);
            _fuelSpawner ??= GetComponentInChildren<FuelSpawner>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_platformData).AsSingle();
            Container.BindInstance(_fuelSpawner).AsSingle();
            BindFuelGrid();
            BindUpgradeLogic();
        }

        private void BindFuelGrid()
        {
            GridStack gridStack = new GridStack(_grid, _platformData.GridData, _prefabs[Prefab.FuelBarrel]);
            Container.BindInstance(gridStack).AsSingle();
            Container.BindInstance(_platformData.GridData).WhenInjectedInto<ClampedBankMaxSign>();
        }

        private void BindUpgradeLogic()
        {
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ClampedBankLeftValueText>();

            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>(); //works
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_receiveZone).AsSingle();
            Container.BindInstance(_preferences).WhenInjectedInto<OilPlatformUpgrader>();
            Container.BindInterfacesTo<OilPlatformUpgrader>().AsSingle();
        }
    }
}