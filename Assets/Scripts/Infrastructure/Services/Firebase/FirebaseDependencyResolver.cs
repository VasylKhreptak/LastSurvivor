using Firebase;
using Firebase.Extensions;
using Infrastructure.Services.Log.Core;
using Zenject;

namespace Infrastructure.Services.Firebase
{
    public class FirebaseDependencyResolver : IInitializable
    {
        private readonly ILogService _logService;

        public FirebaseDependencyResolver(ILogService logService) => _logService = logService;

        public void Initialize() => ResolveDependencies();

        private void ResolveDependencies()
        {
            FirebaseApp
                .CheckAndFixDependenciesAsync()
                .ContinueWithOnMainThread(task =>
                {
                    DependencyStatus dependencyStatus = task.Result;

                    if (dependencyStatus == DependencyStatus.Available)
                        _logService.Log("Firebase dependencies are available");
                    else
                        _logService.LogError("Firebase dependencies are not available");
                });
        }
    }
}