using Firebase;
using Firebase.Extensions;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapFirebaseState : IGameState, IState
    {
        private readonly ILogService _logService;
        private readonly IStateMachine<IGameState> _stateMachine;

        public BootstrapFirebaseState(ILogService logService, IStateMachine<IGameState> stateMachine)
        {
            _logService = logService;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _logService.Log("BootstrapFirebaseState");

            FirebaseApp
                .CheckAndFixDependenciesAsync()
                .ContinueWithOnMainThread(task =>
                {
                    DependencyStatus dependencyStatus = task.Result;

                    if (dependencyStatus == DependencyStatus.Available)
                        _logService.Log("Resolved firebase dependencies");
                    else
                        _logService.LogError("Could not resolve firebase dependencies");

                    _stateMachine.Enter<BootstrapAnalyticsState>();
                });
        }
    }
}