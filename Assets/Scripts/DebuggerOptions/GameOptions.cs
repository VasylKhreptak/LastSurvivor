using System;
using System.ComponentModel;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine.SceneManagement;

namespace DebuggerOptions
{
    public class GameOptions
    {
        private readonly IPersistentDataService _persistentDataService;
        private readonly IStateMachine<IGameState> _stateMachine;

        public GameOptions(IPersistentDataService persistentDataService, IStateMachine<IGameState> stateMachine)
        {
            _persistentDataService = persistentDataService;
            _stateMachine = stateMachine;
        }

        [Category("Game")]
        public void EnterBootstrap() => _stateMachine.Enter<BootstrapState>();

        [Category("Game")]
        public void ReloadScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            LoadSceneWithTransitionAsyncState.Payload payload = new LoadSceneWithTransitionAsyncState.Payload
            {
                LoadScenePayload = new LoadSceneAsyncState.Payload
                {
                    Name = sceneName,
                    OnComplete = null
                }
            };

            _stateMachine.Enter<LoadSceneWithTransitionAsyncState, LoadSceneWithTransitionAsyncState.Payload>(payload);
        }

        [Category("Game")]
        public void LoadCurrentLevel() => _stateMachine.Enter<LoadLevelState, Action>(null);

        [Category("Game")]
        public void LoadPreviousLevel()
        {
            _persistentDataService.Data.PlayerData.CompletedLevelsCount--;

            if (_persistentDataService.Data.PlayerData.CompletedLevelsCount < 0)
                _persistentDataService.Data.PlayerData.CompletedLevelsCount = 0;

            _stateMachine.Enter<LoadLevelState, Action>(null);
        }

        [Category("Game")]
        public void LoadNextLevel()
        {
            _persistentDataService.Data.PlayerData.CompletedLevelsCount++;
            _stateMachine.Enter<LoadLevelState, Action>(null);
        }
    }
}