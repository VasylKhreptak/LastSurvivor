﻿using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadDataState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IPersistentDataService _persistentDataService;

        public LoadDataState(IStateMachine<IGameState> gameStateMachine, IPersistentDataService persistentDataService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            _persistentDataService.Load();
            _gameStateMachine.Enter<SetupApplicationState>();
        }
    }
}