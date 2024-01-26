using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class FinalizeBootstrapState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ILoadingScreen _loadingScreen;

        public FinalizeBootstrapState(IStateMachine<IGameState> stateMachine, IStaticDataService staticDataService,
            ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _loadingScreen = loadingScreen;
        }

        public void Enter()
        {
            LoadSceneAsyncState.Payload payload = new LoadSceneAsyncState.Payload
            {
                Name = _staticDataService.Config.MainScene,
                OnComplete = OnSceneLoaded
            };

            _stateMachine.Enter<LoadSceneAsyncState, LoadSceneAsyncState.Payload>(payload);
        }

        private void OnSceneLoaded() => _loadingScreen.Hide();
    }
}