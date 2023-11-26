using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Platforms.DumpPlatform.Workers.StateMachine;
using Platforms.DumpPlatform.Workers.StateMachine.States;
using Platforms.DumpPlatform.Workers.StateMachine.States.Core;
using UnityEngine;
using Zenject;

namespace Platforms.DumpPlatform.Workers
{
    public class DumpWorkerInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Transform _gearSpawnPoint;

        private DumpPlatformData _platformData;
        private DumpPlatformPreferences _dumpPlatformPreferences;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _platformData = persistentDataService.PersistentData.PlayerData.PlatformsData.DumpPlatformData;
            _dumpPlatformPreferences = staticDataService.Balance.DumpPlatformPreferences;
        }

        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.BindInstance(_platformData).AsSingle();
            Container.BindInstance(_dumpPlatformPreferences).AsSingle();
            BindStateMachine();
        }

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<WorkerStateFactory>().AsSingle();
            Container.BindInterfacesTo<WorkerStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<WorkState>().AsSingle().WithArguments(_gearSpawnPoint);
            Container.Bind<IdleState>().AsSingle();
        }
    }
}