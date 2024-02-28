using Audio;
using DebuggerOptions;
using Infrastructure.Coroutines.Runner;
using Infrastructure.Coroutines.Runner.Core;
using Infrastructure.Data.SaveLoad;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.SceneManagement;
using Infrastructure.SceneManagement.Core;
using Infrastructure.Services.Advertisement;
using Infrastructure.Services.Framerate;
using Infrastructure.Services.ID;
using Infrastructure.Services.ID.Core;
using Infrastructure.Services.Log;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.PersistentData;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.Screen;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.Services.Vibration;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Game.Factory;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.Transition;
using Infrastructure.Transition.Core;
using Plugins.AudioService;
using Settings;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Infrastructure.Zenject.Installers.ProjectContext.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [Header("References")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private GameObject _coroutineRunnerPrefab;
        [SerializeField] private GameObject _loadingScreenPrefab;
        [SerializeField] private GameObject _transitionScreenPrefab;

        [Header("Preferences")]
        [SerializeField] private AudioService.Preferences _audioServicePreferences;
        [SerializeField] private BackgroundMusicPlayer.Preferences _backgroundMusicPreferences;

        public override void InstallBindings()
        {
            BindInstances();
            BindMonoServices();
            BindSceneLoader();
            BindServices();
            BindBackgroundMusic();
            BindGameStateMachine();
            InitializeDebugger();
            MakeInitializable();
        }

        public void Initialize() => BootstrapGame();

        private void BindInstances()
        {
            Container.BindInstance(_audioMixer).AsSingle();
        }

        private void BindMonoServices()
        {
            Container.Bind<ICoroutineRunner>().To<CoroutineRunner>().FromComponentInNewPrefab(_coroutineRunnerPrefab).AsSingle();
            Container.Bind<ILoadingScreen>()
                .To<LoadingScreen.LoadingScreen>()
                .FromComponentInNewPrefab(_loadingScreenPrefab)
                .AsSingle();
            Container.Bind<ITransitionScreen>().To<TransitionScreen>().FromComponentInNewPrefab(_transitionScreenPrefab).AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<IIDService>().To<IDService>().AsSingle();
            Container.Bind<ILogService>().To<LogService>().AsSingle();
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
            Container.Resolve<IStaticDataService>().Load();
            Container.BindInterfacesTo<PersistentDataService>().AsSingle();
            Container.BindInterfacesTo<ApplicationPauseDataSaver>().AsSingle();
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesTo<FramerateService>().AsSingle();
            Container.BindInterfacesTo<ScreenService>().AsSingle();
            Container.BindInterfacesTo<AdvertisementService>().AsSingle();
            Container.BindInterfacesTo<AudioService>().AsSingle().WithArguments(_audioServicePreferences);
            Container.BindInterfacesTo<VibrationService>().AsSingle();
            Container.Bind<SettingsApplier>().AsSingle();
        }

        private void BindBackgroundMusic() =>
            Container.BindInterfacesTo<BackgroundMusicPlayer>().AsSingle().WithArguments(_backgroundMusicPreferences);

        private void BindSceneLoader() => Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();

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
            Container.Bind<SaveDataState>().AsSingle();
            Container.Bind<BootstrapAnalyticsState>().AsSingle();
            Container.Bind<FinalizeBootstrapState>().AsSingle();
            Container.Bind<LoadSceneAsyncState>().AsSingle();
            Container.Bind<LoadSceneWithTransitionAsyncState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            Container.Bind<PlayState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
        }

        private void InitializeDebugger()
        {
            SRDebug.Init();
            SRDebug.Instance.AddOptionContainer(Container.Instantiate<GameOptions>());
            SRDebug.Instance.AddOptionContainer(Container.Instantiate<ResourcesOptions>());
            SRDebug.Instance.AddOptionContainer(Container.Instantiate<AdvertisementOptions>());
        }

        private void MakeInitializable() => Container.Bind<IInitializable>().FromInstance(this).AsSingle();

        private void BootstrapGame() => Container.Resolve<IStateMachine<IGameState>>().Enter<BootstrapState>();
    }
}