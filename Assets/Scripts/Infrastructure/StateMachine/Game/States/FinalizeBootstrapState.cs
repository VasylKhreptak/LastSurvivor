using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class FinalizeBootstrapState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILoadingScreen _loadingScreen;
        private readonly ILogService _logService;

        public FinalizeBootstrapState(IStateMachine<IGameState> stateMachine, ILoadingScreen loadingScreen, ILogService logService)
        {
            _stateMachine = stateMachine;
            _loadingScreen = loadingScreen;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("FinalizeBootstrapState");
            _loadingScreen.SetInfoText("Done!");
            _loadingScreen.Hide();
        }
    }
}