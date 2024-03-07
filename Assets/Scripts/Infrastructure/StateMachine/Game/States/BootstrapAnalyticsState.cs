using System;
using Analytics;
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

        public BootstrapAnalyticsState(IStateMachine<IGameState> stateMachine, ILogService logService,
            DisposableManager disposableManager, DiContainer container)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _disposableManager = disposableManager;
            _container = container;
        }

        private IdleEventLogger _idleEventLogger;
        private ApplicationPauseEventLogger _applicationPauseEventLogger;
        private ApplicationLifetimeEventLogger _applicationLifetimeEventLogger;

        public void Enter()
        {
            _logService.Log("BootstrapAnalyticsState");

            Initialize(ref _idleEventLogger);
            Initialize(ref _applicationPauseEventLogger);
            Initialize(ref _applicationLifetimeEventLogger);

            _stateMachine.Enter<BootstrapCrashlyticsState>();
        }

        private void Initialize<T>(ref T t) where T : IInitializable, IDisposable
        {
            if (t != null)
                return;

            t = _container.Instantiate<T>();
            t.Initialize();
            _disposableManager.Add(t);
        }
    }
}