using DebuggerOptions;
using Infrastructure.Coroutines.Runner;
using Infrastructure.SceneManagement;
using Infrastructure.Services.Advertisement;
using Infrastructure.Services.Framerate;
using Infrastructure.Services.ID;
using Infrastructure.Services.Log;
using Infrastructure.Services.Notification;
using Infrastructure.Services.PersistentData;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.Screen;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.Services.ToastMessage;
using Infrastructure.Services.Vibration;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Game.Factory;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.Transition;
using Observers;
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

        public override void InstallBindings()
        {
            BindInstances();
            BindMonoServices();
            BindSceneLoader();
            BindServices();
            BindObservers();
            BindSettingsApplier();
            BindGameStateMachine();
            InitializeDebugger();
            MakeInitializable();
        }

        public void Initialize() => BootstrapGame();

        private void BindInstances() => Container.BindInstance(_audioMixer).AsSingle();

        private void BindMonoServices()
        {
            Container.BindInterfacesTo<CoroutineRunner>().FromComponentInNewPrefab(_coroutineRunnerPrefab).AsSingle();
            Container.BindInterfacesTo<LoadingScreen.LoadingScreen>().FromComponentInNewPrefab(_loadingScreenPrefab).AsSingle();
            Container.BindInterfacesTo<TransitionScreen>().FromComponentInNewPrefab(_transitionScreenPrefab).AsSingle();
        }

        private void BindSceneLoader() => Container.BindInterfacesTo<SceneLoader>().AsSingle();

        private void BindObservers()
        {
            Container.BindInterfacesAndSelfTo<IdleObserver>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<IDService>().AsSingle();
            Container.BindInterfacesTo<LogService>().AsSingle();
            Container.BindInterfacesTo<ToastMessageService>().AsSingle();
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
            Container.Resolve<IStaticDataService>().Load();
            Container.BindInterfacesTo<PersistentDataService>().AsSingle();
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesTo<FramerateService>().AsSingle();
            Container.BindInterfacesTo<ScreenService>().AsSingle();
            Container.BindInterfacesTo<AdvertisementService>().AsSingle();
            Container.BindInterfacesTo<AudioService>().AsSingle().WithArguments(_audioServicePreferences);
            Container.BindInterfacesTo<VibrationService>().AsSingle();
            Container.BindInterfacesTo<NotificationService>().AsSingle();
        }

        private void BindSettingsApplier() => Container.Bind<SettingsApplier>().AsSingle();

        private void BindGameStateMachine()
        {
            BindGameStates();
            Container.Bind<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
        }

        private void BindGameStates()
        {
            //chained
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<BootstrapPlayServicesState>().AsSingle();
            Container.Bind<LoginState>().AsSingle();
            Container.Bind<LoadDataState>().AsSingle();
            Container.Bind<ApplySavedSettingsState>().AsSingle();
            Container.Bind<BootstrapFirebaseState>().AsSingle();
            Container.Bind<BootstrapAnalyticsState>().AsSingle();
            Container.Bind<BootstrapCrashlyticsState>().AsSingle();
            Container.Bind<BootstrapAdvertisementsState>().AsSingle();
            Container.Bind<BootstrapMessagingState>().AsSingle();
            Container.Bind<SetupAutomaticDataSaveState>().AsSingle();
            Container.Bind<ScheduleNotificationsState>().AsSingle();
            Container.Bind<LoadMainSceneState>().AsSingle();
            Container.Bind<SetupBackgroundMusicState>().AsSingle();
            Container.Bind<FinalizeBootstrapState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            //other
            Container.Bind<ReloadState>().AsSingle();
            Container.Bind<SaveDataState>().AsSingle();
            Container.Bind<LoadSceneAsyncState>().AsSingle();
            Container.Bind<LoadSceneWithTransitionAsyncState>().AsSingle();
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