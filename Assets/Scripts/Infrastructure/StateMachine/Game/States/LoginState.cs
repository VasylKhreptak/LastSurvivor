using System;
using Firebase.Auth;
using GooglePlayGames;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Observers;
using UnityEngine;

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

            Login(EnterNextState);
        }

        private void Login(Action onComplete)
        {
            Social.localUser.Authenticate(isAuthenticated =>
            {
                if (isAuthenticated == false)
                {
                    _logService.Log("Authentication failed");
                    onComplete();
                    return;
                }

                string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                LoginFirebase(authCode, onComplete);
            });
        }

        private void LoginFirebase(string authCode, Action onComplete)
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
            auth.SignInAndRetrieveDataWithCredentialAsync(credential)
                .ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        _logService.Log("Login canceled");
                        onComplete();
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        _logService.Log($"Login failed: {task.Exception}");
                        onComplete();
                        return;
                    }

                    AuthResult result = task.Result;
                    _logService.Log($"Login successful. DisplayName: {result.User.DisplayName}. ID: {result.User.UserId}");
                    onComplete();
                });
        }

        private void EnterNextState() => _stateMachine.Enter<LoadDataState>();
    }
}