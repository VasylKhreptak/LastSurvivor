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

        public BootstrapAdvertisementsState(IStateMachine<IGameState> stateMachine, ILogService logService,
            IAdvertisementService advertisementService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _advertisementService = advertisementService;
        }

        public void Enter()
        {
            _logService.Log("BootstrapAdvertisementsState");

            _advertisementService.Initialize(status =>
            {
                if (status == null)
                    _logService.LogError("Failed to initialize advertisement service");
                else
                    _logService.Log("Advertisement service initialized");

                _stateMachine.Enter<SetupAutomaticDataSaveState>();
            });
        }
    }
}