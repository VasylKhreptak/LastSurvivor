using System.Threading.Tasks;
using Firebase.Auth;
using GooglePlayGames;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;
using Utilities.Networking;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoginState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;
        private readonly ILoadingScreen _loadingScreen;

        public LoginState(IStateMachine<IGameState> stateMachine, ILogService logService, ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _loadingScreen = loadingScreen;
        }

        public async void Enter()
        {
            _logService.Log("LoginState");
            _loadingScreen.SetInfoText("Logging in...");

            await Login();

            EnterNextState();
        }

        private async Task Login()
        {
            if (await InternetConnection.CheckAsync() == false)
            {
                _logService.Log("No internet connection");
                return;
            }

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Social.localUser.Authenticate(async isAuthenticated =>
            {
                await OnAuthenticationFinished(isAuthenticated);
                tcs.SetResult(true);
            });

            await tcs.Task;
        }

        private async Task OnAuthenticationFinished(bool isAuthenticated)
        {
            if (isAuthenticated == false)
            {
                _logService.Log("Authentication failed");
                return;
            }

            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            await LoginFirebase(authCode);
        }

        private async Task LoginFirebase(string authCode)
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
            AuthResult authResult = await auth.SignInAndRetrieveDataWithCredentialAsync(credential);
            FirebaseUser user = authResult.User;

            if (user != null)
            {
                _logService.Log($"Login successful. DisplayName: {user.DisplayName}. ID: {user.UserId}");
                return;
            }

            _logService.Log("Login failed");
        }

        private void EnterNextState() => _stateMachine.Enter<LoadDataState>();
    }
}