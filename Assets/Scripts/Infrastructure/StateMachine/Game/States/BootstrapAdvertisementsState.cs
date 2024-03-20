using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Advertisement.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapAdvertisementsState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;
        private readonly IAdvertisementService _advertisementService;
        private readonly ILoadingScreen _loadingScreen;

        public BootstrapAdvertisementsState(IStateMachine<IGameState> stateMachine, ILogService logService,
            IAdvertisementService advertisementService, ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _advertisementService = advertisementService;
            _loadingScreen = loadingScreen;
        }

        public void Enter()
        {
            _logService.Log("BootstrapAdvertisementsState");
            _loadingScreen.SetInfoText("Initializing advertisements...");

            _advertisementService.Initialize(status =>
            {
                if (status == null)
                    _logService.LogError("Failed to initialize advertisement service");
                else
                    _logService.Log("Advertisement service initialized");

                EnterNextState();
            });
        }

        private void EnterNextState() => _stateMachine.Enter<BootstrapMessagingState>();
    }
}