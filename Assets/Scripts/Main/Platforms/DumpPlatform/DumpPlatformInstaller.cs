using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using Flexalon;
using Grid;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Main.Platforms.RecruitmentLogic;
using Main.Platforms.Zones;
using Plugins.Banks;
using UI.ClampedBanks;
using UnityEngine;
using Zenject;

namespace Main.Platforms.DumpPlatform
{
    public class DumpPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private ReceiveZone _hireWorkerZone;
        [SerializeField] private EntityRecruiter _workerRecruiter;
        [SerializeField] private FlexalonGridLayout _collectZone;

        private IntegerBank _bank;
        private ClampedIntegerBank _hireWorkerContainer;
        private DumpPlatformData _platformData;
        private GamePrefabs _prefabs;
        private DumpPlatformPreferences _platformPreferences;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _hireWorkerContainer = persistentDataService.PersistentData.PlayerData.PlatformsData.DumpPlatformData.HireWorkerContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.PlatformsData.DumpPlatformData;
            _prefabs = staticDataService.Prefabs;
            _platformPreferences = staticDataService.Balance.DumpPlatformPreferences;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _hireWorkerZone ??= GetComponentInChildren<ReceiveZone>(true);
            _workerRecruiter ??= GetComponentInChildren<EntityRecruiter>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_platformData).AsSingle();
            BindGearsCollectZone();
            BindHireWorkerZone();
            BindWorkersCountText();
            BindWorkersRecruiter();
            BindWorkerPriceUpdater();
            BindReceiveZoneVibration();
        }

        private void BindHireWorkerZone()
        {
            Container.BindInstance(_platformData.WorkersBank).WhenInjectedInto<ReceiveZoneDrawer>();
            Container.BindInstance(_hireWorkerContainer).WhenInjectedInto<ClampedBankLeftValueText>();
            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_hireWorkerContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_hireWorkerZone).AsSingle();
        }

        private void BindWorkersCountText() =>
            Container.BindInstance(_platformData.WorkersBank).WhenInjectedInto<ClampedBankValuesText>();

        private void BindWorkersRecruiter()
        {
            Container.BindInstance(_platformData.WorkersBank).WhenInjectedInto<EntityRecruiter>();
            Container.BindInstance(_workerRecruiter).AsSingle();
        }

        private void BindWorkerPriceUpdater()
        {
            Container
                .BindInterfacesTo<EntityPriceUpdater>()
                .AsSingle()
                .WithArguments(_platformData.WorkersBank,
                    _platformData.HireWorkerContainer,
                    _platformPreferences.WorkerHirePricePreferences);
        }

        private void BindGearsCollectZone()
        {
            GridStack gridStack = new GridStack(_collectZone, _platformData.GearsBank, _prefabs[Prefab.Gear]);
            Container.BindInstance(gridStack).AsSingle();
            Container.BindInstance(_bank).WhenInjectedInto<CollectZone>();
            Container.BindInstance(_platformData.GearsBank).WhenInjectedInto<ClampedBankMaxSign>();
        }

        private void BindReceiveZoneVibration() => Container.BindInterfacesTo<ReceiveZoneVibration>().AsSingle();
    }
}