using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Main.Platforms.DumpPlatform.Workers.StateMachine;
using Main.Platforms.DumpPlatform.Workers.StateMachine.States;
using Main.Platforms.DumpPlatform.Workers.StateMachine.States.Core;
using UnityEngine;
using Zenject;

namespace Main.Platforms.DumpPlatform.Workers
{
    public class WorkerInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private WorkerReferences _workerReferences;

        private DumpPlatformData _platformData;
        private DumpPlatformPreferences _dumpPlatformPreferences;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _platformData = persistentDataService.Data.PlayerData.PlatformsData.DumpPlatformData;
            _dumpPlatformPreferences = staticDataService.Balance.DumpPlatformPreferences;
        }

        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.BindInstance(_workerReferences).AsSingle();
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
            Container.BindInterfacesAndSelfTo<WorkState>().AsSingle();
            Container.Bind<IdleState>().AsSingle();
        }
    }
}