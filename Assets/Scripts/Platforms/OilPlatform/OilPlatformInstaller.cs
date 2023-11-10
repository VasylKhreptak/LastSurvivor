using Data.Persistent;
using Data.Static.Balance.Upgrade;
using Flexalon;
using Grid;
using Infrastructure.Data.Static;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Plugins.Banks;
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
            Container.BindInstance(_bank).AsSingle();
            Container.BindInstance(_upgradeContainer).AsSingle();
            Container.BindInstance(_platformData).AsSingle();
            Container.BindInstance(_platformData.GridData).AsSingle();
            Container.BindInstance(_grid).AsSingle();
            BindFuelGrid();
            BindUpgradeLogic();
        }

        private void BindFuelGrid()
        {
            GridStack gridStack = Container.Instantiate<GridStack>();
            gridStack.LoadFromGridData(_gamePrefabs.FuelBarrel);
            Container.BindInstance(gridStack).AsSingle();
        }

        private void BindUpgradeLogic()
        {
            Container.BindInstance(_upgradePreferences).AsSingle();
            Container.BindInstance(_gamePrefabs.Gear).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_receiveZone).AsSingle();
            Container.BindInterfacesTo<OilPlatformUpgrader>().AsSingle();
        }
    }
}