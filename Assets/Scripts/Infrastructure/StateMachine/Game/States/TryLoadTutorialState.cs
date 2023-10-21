using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class TryLoadTutorialState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentDataService _persistentDataService;

        public TryLoadTutorialState(IStateMachine<IGameState> stateMachine,
            IStaticDataService staticDataService,
            IPersistentDataService persistentDataService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            // bool finishedTutorial = _runtimeDataService.RuntimeData.PlayerData.FinishedTutorial;

            bool finishedTutorial = true;

            string nextSceneName =
                finishedTutorial ? _staticDataService.Config.MainScene : _staticDataService.Config.TutorialScene;

            _stateMachine.Enter<LoadLevelState, string>(nextSceneName);
        }
    }
}