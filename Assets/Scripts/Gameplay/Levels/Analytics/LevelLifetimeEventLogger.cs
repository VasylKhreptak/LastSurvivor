using System;
using Firebase.Analytics;
using Zenject;

namespace Gameplay.Levels.Analytics
{
    public class LevelLifetimeEventLogger : IInitializable, IDisposable
    {
        public void Initialize() => FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);

        public void Dispose() => FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);
    }
}