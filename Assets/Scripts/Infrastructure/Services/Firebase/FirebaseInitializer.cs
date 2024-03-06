using Crashlytics;
using Firebase;
using Firebase.Extensions;
using Infrastructure.Services.Log.Core;
using Zenject;

namespace Infrastructure.Services.Firebase
{
    public class FirebaseInitializer : IInitializable
    {
        private readonly ILogService _logService;
        private readonly CrashlyticsInitializer _crashlyticsInitializer;

        public FirebaseInitializer(ILogService logService, CrashlyticsInitializer crashlyticsInitializer)
        {
            _logService = logService;
            _crashlyticsInitializer = crashlyticsInitializer;
        }

        public void Initialize()
        {
            FirebaseApp
                .CheckAndFixDependenciesAsync()
                .ContinueWithOnMainThread(task =>
                {
                    DependencyStatus dependencyStatus = task.Result;

                    if (dependencyStatus == DependencyStatus.Available)
                        _logService.Log("Resolved firebase dependencies");
                    else
                        _logService.LogError("Could not resolve firebase dependencies");

                    _crashlyticsInitializer.Initialize();
                });
        }
    }
}