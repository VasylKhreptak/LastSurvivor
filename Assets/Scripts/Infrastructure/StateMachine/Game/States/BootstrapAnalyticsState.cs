﻿using Infrastructure.Services.RuntimeData.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapAnalyticsState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IRuntimeDataService _runtimeDataService;

        public BootstrapAnalyticsState(IStateMachine<IGameState> gameStateMachine,
            IRuntimeDataService runtimeDataService)
        {
            _gameStateMachine = gameStateMachine;
            _runtimeDataService = runtimeDataService;
        }

        public void Enter()
        {
            _runtimeDataService.RuntimeData.AnalyticsData.SessionsCount++;
            _gameStateMachine.Enter<TryLoadTutorialState>();
        }
    }
}
