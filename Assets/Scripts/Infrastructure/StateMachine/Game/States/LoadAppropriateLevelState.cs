using System;
using System.Linq;
using Gameplay.Levels;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadAppropriateLevelState : IPayloadedState<Action>, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentDataService _persistentDataService;

        public LoadAppropriateLevelState(IStateMachine<IGameState> stateMachine, IStaticDataService staticDataService,
            IPersistentDataService persistentDataService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _persistentDataService = persistentDataService;
        }

        public void Enter(Action onComplete)
        {
            string sceneName = GetAppropriateLevel().Scene.Name;

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

        private Level GetAppropriateLevel()
        {
            int completedLevels = _persistentDataService.PersistentData.PlayerData.CompletedLevels;

            if (completedLevels < _staticDataService.Config.Levels.Count)
                return _staticDataService.Config.Levels[completedLevels];

            if (_staticDataService.Config.LoopedLevels.Count == 0)
                return _staticDataService.Config.Levels.Last();

            return _staticDataService.Config.LoopedLevels[completedLevels % _staticDataService.Config.LoopedLevels.Count];
        }
    }
}