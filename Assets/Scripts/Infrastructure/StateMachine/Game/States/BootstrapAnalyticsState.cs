using System;
using Analytics;
using Firebase.Analytics;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapAnalyticsState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;
        private readonly DisposableManager _disposableManager;
        private readonly DiContainer _container;
        private readonly ILoadingScreen _loadingScreen;

        public BootstrapAnalyticsState(IStateMachine<IGameState> stateMachine, ILogService logService,
            DisposableManager disposableManager, DiContainer container, ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _disposableManager = disposableManager;
            _container = container;
            _loadingScreen = loadingScreen;
        }

        private bool _initialized;

        private IdleEventLogger _idleEventLogger;
        private ApplicationPauseEventLogger _applicationPauseEventLogger;
        private ResourcesCountEventLogger _resourcesCountEventLogger;

        public void Enter()
        {
            _logService.Log("BootstrapAnalyticsState");
            _loadingScreen.SetInfoText("Initializing analytics...");

            LogApplicationOpenEvent();
            Initialize(ref _idleEventLogger);
            Initialize(ref _applicationPauseEventLogger);
            Initialize(ref _resourcesCountEventLogger);

            EnterNextState();
        }

        private void EnterNextState() => _stateMachine.Enter<BootstrapCrashlyticsState>();

        private void Initialize<T>(ref T t) where T : IInitializable, IDisposable
        {
            if (t != null)
                _disposableManager.Remove(t);

            t ??= _container.Instantiate<T>();
            t.Dispose();
            t.Initialize();
            _disposableManager.Add(t);
        }

        private void LogApplicationOpenEvent() => FirebaseAnalytics.LogEvent(AnalyticEvents.ApplicationOpen);
    }
}