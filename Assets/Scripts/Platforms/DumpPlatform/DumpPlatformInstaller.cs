using Data.Persistent.Platforms;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
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

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _hireWorkerContainer = persistentDataService.PersistentData.PlayerData.DumpPlatformData.HireWorkerContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.DumpPlatformData;
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
            Container.BindInstance(_hireWorkerContainer).WhenInjectedInto<WorkersRecruiter>();
        }
    }
}