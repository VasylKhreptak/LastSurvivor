using GooglePlayGames;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoginState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;

        public LoginState(IStateMachine<IGameState> stateMachine, ILogService logService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("LoginState");

            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        private void ProcessAuthentication(bool authenticated)
        {
            _logService.Log("Logged in: " + authenticated);
            EnterNextState();
        }

        private void EnterNextState() => _stateMachine.Enter<LoadDataState>();
    }
}