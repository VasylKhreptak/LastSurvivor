using Data.Persistent.Platforms;
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

namespace Platforms.DumpPlatform
{
    public class DumpPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private ReceiveZone _hireWorkerZone;
        [SerializeField] private WorkersRecruiter _workersRecruiter;
        [SerializeField] private FlexalonGridLayout _collectZone;

        private IntegerBank _bank;
        private ClampedIntegerBank _hireWorkerContainer;
        private DumpPlatformData _platformData;
        private GamePrefabs _prefabs;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _hireWorkerContainer = persistentDataService.PersistentData.PlayerData.DumpPlatformData.HireWorkerContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.DumpPlatformData;
            _prefabs = staticDataService.Prefabs;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _hireWorkerZone ??= GetComponentInChildren<ReceiveZone>(true);
            _workersRecruiter ??= GetComponentInChildren<WorkersRecruiter>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_platformData).AsSingle();
            BindGearsCollectZone();
            BindHireWorkerZone();
            BindWorkersCountText();
            BindWorkersRecruiter();
        }

        private void BindHireWorkerZone()
        {
            Container.BindInstance(_hireWorkerContainer).WhenInjectedInto<ClampedBankLeftValueText>();

            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_hireWorkerContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_hireWorkerZone).AsSingle();
        }

        private void BindWorkersCountText()
        {
            Container.BindInstance(_platformData.WorkersBank).WhenInjectedInto<ClampedBankValuesText>();
        }

        private void BindWorkersRecruiter()
        {
            Container.BindInstance(_workersRecruiter).AsSingle();
            Container.BindInstance(_hireWorkerContainer).WhenInjectedInto<WorkerPriceIncrementor>();
            Container.BindInterfacesTo<WorkerPriceIncrementor>().AsSingle();
        }

        private void BindGearsCollectZone()
        {
            GridStack gridStack = new GridStack(_collectZone, _platformData.GridData, _prefabs[Prefab.Gear]);
            Container.BindInstance(gridStack).AsSingle();
            Container.BindInstance(_bank).WhenInjectedInto<CollectZone>();
            Container.BindInstance(_platformData.GridData).WhenInjectedInto<ClampedBankMaxSign>();
        }
    }
}