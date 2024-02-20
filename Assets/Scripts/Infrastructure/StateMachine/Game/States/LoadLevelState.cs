using System;
using System.Linq;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadLevelState : IPayloadedState<Action>, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentDataService _persistentDataService;

        public LoadLevelState(IStateMachine<IGameState> stateMachine, IStaticDataService staticDataService,
            IPersistentDataService persistentDataService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _persistentDataService = persistentDataService;
        }

        public void Enter(Action onComplete)
        {
            string sceneName = GetAppropriateScene();

            LoadSceneWithTransitionAsyncState.Payload payload = new LoadSceneWithTransitionAsyncState.Payload
            {
                LoadScenePayload = new LoadSceneAsyncState.Payload
                {
                    Name = sceneName,
                    OnComplete = onComplete
                }
            };

            _stateMachine.Enter<LoadSceneWithTransitionAsyncState, LoadSceneWithTransitionAsyncState.Payload>(payload);
        }

        private string GetAppropriateScene()
        {
            int completedLevels = _persistentDataService.Data.PlayerData.CompletedLevels;

            if (completedLevels < _staticDataService.Config.Levels.Count)
                return _staticDataService.Config.Levels[completedLevels].Name;

            if (_staticDataService.Config.LoopedLevels.Count == 0)
                return _staticDataService.Config.Levels.Last().Name;

            int loopedLevelIndex = (completedLevels - _staticDataService.Config.Levels.Count) %
                                   _staticDataService.Config.LoopedLevels.Count;

            return _staticDataService.Config.LoopedLevels[loopedLevelIndex].Name;
        }
    }
}