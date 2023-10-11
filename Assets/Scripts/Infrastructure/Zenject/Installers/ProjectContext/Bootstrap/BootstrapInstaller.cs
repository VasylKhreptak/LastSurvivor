using DebuggerOptions;
using Infrastructure.Coroutines.Runner;
using Infrastructure.Coroutines.Runner.Core;
using Infrastructure.Curtain;
using Infrastructure.Curtain.Core;
using Infrastructure.SceneManagement;
using Infrastructure.SceneManagement.Core;
using Infrastructure.Services.Framerate;
using Infrastructure.Services.ID;
using Infrastructure.Services.ID.Core;
using Infrastructure.Services.Log;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.RuntimeData;
using Infrastructure.Services.RuntimeData.Core;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.SaveLoad.Core;
using Infrastructure.Services.SaveLoadHandler;
using Infrastructure.Services.Screen;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Game.Factory;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.Zenject.Installers.ProjectContext.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [Header("References")]
        [SerializeField] private GameObject _coroutineRunnerPrefab;
        [SerializeField] private GameObject _loadingCurtainPrefab;

        public override void InstallBindings()
        {
            BindMonoServices();
            BindSceneLoader();
            BindServices();
            BindGameStateMachine();
            InitializeDebugger();
            MakeInitializable();
        }

        public void Initialize() => BootstrapGame();

        private void BindMonoServices()
        {
            Container.Bind<ICoroutineRunner>().To<CoroutineRunner>().FromComponentInNewPrefab(_coroutineRunnerPrefab).AsSingle();
            Container.Bind<ILoadingScreen>().To<LoadingScreen>().FromComponentInNewPrefab(_loadingCurtainPrefab).AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<IIDService>().To<IDService>().AsSingle();
            Container.Bind<ILogService>().To<LogService>().AsSingle();

            IStaticDataService staticDataService = Container.Instantiate<StaticDataService>();
            Container.Bind<IStaticDataService>().FromInstance(staticDataService).AsSingle();
            staticDataService.Load();

            Container.Bind<IRuntimeDataService>().To<RuntimeDataService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.BindInterfacesTo<FramerateService>().AsSingle();
            Container.BindInterfacesTo<ScreenService>().AsSingle();

            BindSaveLoadHandlerService();
        }

        private void BindSaveLoadHandlerService()
        {
            SaveLoadHandlerService saveLoadHandlerService = Container.Instantiate<SaveLoadHandlerService>();

            saveLoadHandlerService.Add(Container.Resolve<IRuntimeDataService>());

            Container.BindInterfacesTo<SaveLoadHandlerService>().FromInstance(saveLoadHandlerService).AsSingle();
        }

        private void BindSceneLoader()
        {
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            BindGameStates();
        }

        private void BindGameStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<SetupApplicationState>().AsSingle();
            Container.Bind<LoadDataState>().AsSingle();
            Container.Bind<BootstrapAnalyticsState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
        }

        private void BootstrapGame() => Container.Resolve<IStateMachine<IGameState>>().Enter<BootstrapState>();

        private void InitializeDebugger()
        {
            SRDebug.Init();
            SRDebug.Instance.AddOptionContainer(Container.Instantiate<SROptionsContainer>());
        }

        private void MakeInitializable()
        {
            Container.Bind<IInitializable>().FromInstance(this).AsSingle();
        }
    }
}
