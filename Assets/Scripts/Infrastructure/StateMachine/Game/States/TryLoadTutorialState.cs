using Infrastructure.Services.RuntimeData.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class TryLoadTutorialState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IRuntimeDataService _runtimeDataService;

        public TryLoadTutorialState(IStateMachine<IGameState> stateMachine,
            IStaticDataService staticDataService,
            IRuntimeDataService runtimeDataService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _runtimeDataService = runtimeDataService;
        }

        public void Enter()
        {
            // bool finishedTutorial = _runtimeDataService.RuntimeData.PlayerData.FinishedTutorial;

            bool finishedTutorial = true;

            string nextSceneName = finishedTutorial ? _staticDataService.Config.MainScene : _staticDataService.Config.TutorialScene;

            _stateMachine.Enter<LoadLevelState, string>(nextSceneName);
        }
    }
}
