using System;
using Analytics;
using Firebase.Analytics;

namespace Gameplay.Levels.Analytics
{
    public class FailedFirstLevelEventLogger : IDisposable
    {
        private readonly LevelManager _levelManager;

        public FailedFirstLevelEventLogger(LevelManager levelManager) => _levelManager = levelManager;

        public void Dispose()
        {
            if (_levelManager.GetCurrentLevel() == 1)
                FirebaseAnalytics.LogEvent(AnalyticEvents.FailedFirstLevel);
        }
    }
}