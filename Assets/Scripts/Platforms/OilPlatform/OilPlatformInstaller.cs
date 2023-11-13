using Data.Persistent;
using Data.Static.Balance.Upgrade;
using Flexalon;
using Grid;
using Infrastructure.Data.Static;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Plugins.Banks;
using UI.ClampedBanks;
using UI.Grid;
using UnityEngine;
using Zenject;

namespace Platforms.OilPlatform
{
    public class OilPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private FlexalonGridLayout _grid;
        [SerializeField] private ReceiveZone _receiveZone;

        private IntegerBank _bank;
        private ClampedIntegerBank _upgradeContainer;
        private OilPlatformData _platformData;
        private GamePrefabs _gamePrefabs;
        private OilPlatformUpgradePreferences _upgradePreferences;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _upgradeContainer = persistentDataService.PersistentData.PlayerData.OilPlatformData.UpgradeContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.OilPlatformData;
            _gamePrefabs = staticDataService.Prefabs;
            _upgradePreferences = staticDataService.Balance.OilPlatformUpgradePreferences;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_platformData).AsSingle();
            BindFuelGrid();
            BindUpgradeLogic();
        }

        private void BindFuelGrid()
        {
            Container.BindInstance(_grid).WhenInjectedInto<GridStack>();
            Container.BindInstance(_platformData.GridData).WhenInjectedInto<GridStack>();
            GridStack gridStack = Container.Instantiate<GridStack>();
            gridStack.LoadWith(_gamePrefabs.FuelBarrel);
            Container.BindInstance(gridStack).AsSingle();
            Container.BindInstance(_platformData.GridData).WhenInjectedInto<ClampedBankMaxSign>();
        }

        private void BindUpgradeLogic()
        {
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ClampedBankLeftValueText>();

            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>(); //works
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_gamePrefabs.Gear).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_receiveZone).AsSingle();

            // Container
            //     .BindInstance(
            //         _receiveZone) //do not work.Unable to resolve IntegerBank(_bank) when building object with type ReceiveZone
            //     .AsSingle()
            //     .WithArguments(_bank, _upgradeContainer, _gamePrefabs.Gear);

            Container.BindInstance(_upgradePreferences).WhenInjectedInto<OilPlatformUpgrader>();
            Container.BindInterfacesTo<OilPlatformUpgrader>().AsSingle();
        }
    }
}