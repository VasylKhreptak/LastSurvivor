using System;
using Analytics;
using Firebase.Analytics;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapAnalyticsState : IState, IGameState, IDisposable
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IPersistentDataService _persistentDataService;

        public BootstrapAnalyticsState(IStateMachine<IGameState> gameStateMachine, IPersistentDataService persistentDataService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
        }

        private ResourcesCountEventLogger _resourcesCountEventLogger;

        public void Enter()
        {
            _persistentDataService.Data.AnalyticsData.SessionsCount++;

            FirebaseAnalytics.LogEvent(AnalyticEvents.ApplicationOpen,
                new Parameter(AnalyticParameters.Count, _persistentDataService.Data.AnalyticsData.SessionsCount));

            InitializeResourcesCountEventLogger();

            _gameStateMachine.Enter<FinalizeBootstrapState>();
        }

        public void Dispose() => DisposeResourcesCountAnalytics();

        private void InitializeResourcesCountEventLogger()
        {
            _resourcesCountEventLogger ??= new ResourcesCountEventLogger(_persistentDataService);
            _resourcesCountEventLogger.Dispose();
            _resourcesCountEventLogger.Initialize();
        }

        private void DisposeResourcesCountAnalytics() => _resourcesCountEventLogger?.Dispose();
    }
}