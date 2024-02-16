using Gameplay.Data;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Gameplay.Levels.StateMachine.States
{
    public class FinalizeProgressAndLoadMenuState : ILevelState, IState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly LevelData _levelData;
        private readonly IPersistentDataService _persistentDataService;
        private readonly IStaticDataService _staticDataService;

        public FinalizeProgressAndLoadMenuState(IStateMachine<IGameState> gameStateMachine, LevelData levelData,
            IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _levelData = levelData;
            _persistentDataService = persistentDataService;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            LoadSceneWithTransitionAsyncState.Payload payload = new LoadSceneWithTransitionAsyncState.Payload
            {
                LoadScenePayload = new LoadSceneAsyncState.Payload
                {
                    Name = _staticDataService.Config.MainScene.Name
                },

                OnTransitionShown = FinalizeProgress
            };

            _gameStateMachine.Enter<LoadSceneWithTransitionAsyncState, LoadSceneWithTransitionAsyncState.Payload>(payload);
        }

        private void FinalizeProgress()
        {
            _persistentDataService.Data.PlayerData.Resources.Money.Add(_levelData.CollectedMoney.Value);
            _persistentDataService.Data.PlayerData.Resources.Gears.Add(_levelData.CollectedGears.Value);
        }
    }
}