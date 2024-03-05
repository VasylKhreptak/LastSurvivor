using System;
using System.Diagnostics;
using Analytics;
using Firebase.Analytics;
using Gameplay.Data;
using Zenject;

namespace Gameplay.Levels.Analytics
{
    public class LevelEventLogger : IInitializable, IDisposable
    {
        private readonly LevelManager _levelManager;
        private readonly LevelData _levelData;

        public LevelEventLogger(LevelManager levelManager, LevelData levelData)
        {
            _levelManager = levelManager;
            _levelData = levelData;
        }

        private readonly Stopwatch _stopwatch = new Stopwatch();

        public void Initialize()
        {
            _stopwatch.Start();
            LogLevelLoadedEvent();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            LogLevelEndedEvent();
        }

        private void LogLevelLoadedEvent() => FirebaseAnalytics.LogEvent(AnalyticEvents.LevelLoaded, CreateLevelCountParameter());

        public void LogLevelStartedEvent() =>
            FirebaseAnalytics.LogEvent(AnalyticEvents.LevelStarted, CreateLevelCountParameter(), CreateTimeParameter());

        public void LogLevelCompletedEvent() =>
            FirebaseAnalytics.LogEvent(AnalyticEvents.LevelCompleted, CreateLevelCountParameter(), CreateTimeParameter());

        public void LogLevelFailedEvent() =>
            FirebaseAnalytics.LogEvent(AnalyticEvents.LevelFailed, CreateLevelCountParameter(), CreateTimeParameter());

        private void LogLevelEndedEvent()
        {
            FirebaseAnalytics.LogEvent(AnalyticEvents.LevelEnded,
                CreateLevelCountParameter(),
                CreateTimeParameter(),
                new Parameter("Collected money", _levelData.CollectedMoney.Value),
                new Parameter("Collected gears", _levelData.CollectedGears.Value),
                new Parameter("Level result", _levelData.LevelResult.ToString()));
        }

        private Parameter CreateLevelCountParameter() => new Parameter(AnalyticParameters.Level, _levelManager.GetCurrentLevel());

        private Parameter CreateTimeParameter() => new Parameter(AnalyticParameters.Time, _stopwatch.Elapsed.TotalSeconds);
    }
}