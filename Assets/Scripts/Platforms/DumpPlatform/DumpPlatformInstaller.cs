using Data.Persistent.Platforms;
using Infrastructure.Data.Static;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
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

        private IntegerBank _bank;
        private ClampedIntegerBank _hireWorkerContainer;
        private DumpPlatformData _platformData;
        private GamePrefabs _gamePrefabs;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _hireWorkerContainer = persistentDataService.PersistentData.PlayerData.OilPlatformData.UpgradeContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.DumpPlatformData;
            _gamePrefabs = staticDataService.Prefabs;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _hireWorkerZone ??= GetComponentInChildren<ReceiveZone>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_platformData).AsSingle();
            BindHireWorkerZone();
            BindWorkersCountText();
        }

        private void BindHireWorkerZone()
        {
            Container.BindInstance(_hireWorkerContainer).WhenInjectedInto<ClampedBankLeftValueText>();

            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_hireWorkerContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_gamePrefabs.Gear).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_hireWorkerZone).AsSingle();
        }

        private void BindWorkersCountText()
        {
            Container.BindInstance(_platformData.WorkersBank).WhenInjectedInto<ClampedBankValuesText>();
        }
    }
}