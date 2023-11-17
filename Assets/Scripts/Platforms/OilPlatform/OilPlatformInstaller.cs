using Data.Persistent.Platforms;
using Data.Static.Balance.Upgrade;
using Flexalon;
using Grid;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
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
        private OilPlatformUpgradePreferences _upgradePreferences;
        private GamePrefabs _prefabs;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _upgradeContainer = persistentDataService.PersistentData.PlayerData.OilPlatformData.UpgradeContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.OilPlatformData;
            _upgradePreferences = staticDataService.Balance.OilPlatformUpgradePreferences;
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
            Container.BindInstance(_grid).WhenInjectedInto<GridStack>();
            Container.BindInstance(_platformData.GridData).WhenInjectedInto<GridStack>();
            GridStack gridStack = Container.Instantiate<GridStack>();
            gridStack.LoadWith(_prefabs[Prefab.FuelBarrel]);
            Container.BindInstance(gridStack).AsSingle();
            Container.BindInstance(_platformData.GridData).WhenInjectedInto<ClampedBankMaxSign>();
        }

        private void BindUpgradeLogic()
        {
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ClampedBankLeftValueText>();

            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>(); //works
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_receiveZone).AsSingle();
            Container.BindInstance(_upgradePreferences).WhenInjectedInto<OilPlatformUpgrader>();
            Container.BindInterfacesTo<OilPlatformUpgrader>().AsSingle();
        }
    }
}