﻿using System;
using Infrastructure.SceneManagement.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadSceneAsyncState : IPayloadedState<LoadSceneAsyncState.Payload>, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;

        public LoadSceneAsyncState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(Payload payload)
        {
            _sceneLoader.LoadAsync(payload.Name, () => OnLoadedScene(payload));
        }

        private void OnLoadedScene(Payload payload)
        {
            _gameStateMachine.Enter<GameLoopState>();
            payload.OnComplete?.Invoke();
        }

        public class Payload
        {
            public string Name;
            public Action OnComplete;
        }
    }
}