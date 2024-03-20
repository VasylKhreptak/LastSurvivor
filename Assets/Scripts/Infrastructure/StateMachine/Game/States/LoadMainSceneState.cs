using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadMainSceneState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ILoadingScreen _loadingScreen;
        private readonly ILogService _logService;

        public LoadMainSceneState(IStateMachine<IGameState> stateMachine, IStaticDataService staticDataService,
            ILoadingScreen loadingScreen, ILogService logService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _loadingScreen = loadingScreen;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("LoadMainSceneState");
            _loadingScreen.SetInfoText("Loading main scene...");

            LoadSceneAsyncState.Payload payload = new LoadSceneAsyncState.Payload
            {
                Name = _staticDataService.Config.MainScene.Name, OnComplete = EnterNextState
            };

            _stateMachine.Enter<LoadSceneAsyncState, LoadSceneAsyncState.Payload>(payload);
        }

        private void EnterNextState() => _stateMachine.Enter<FinalizeBootstrapState>();
    }
}