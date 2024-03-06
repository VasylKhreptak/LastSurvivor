using Infrastructure.Services.Log.Core;
using Zenject;

namespace Crashlytics
{
    public class CrashlyticsInitializer : IInitializable
    {
        private readonly ILogService _logService;

        public CrashlyticsInitializer(ILogService logService)
        {
            _logService = logService;
        }

        public void Initialize()
        {
            Firebase.Crashlytics.Crashlytics.IsCrashlyticsCollectionEnabled = true;
            Firebase.Crashlytics.Crashlytics.ReportUncaughtExceptionsAsFatal = true;
            _logService.Log("Crashlytics initialized");
        }
    }
}