using System;
using System.Diagnostics;
using Firebase.Analytics;
using UnityEngine.SceneManagement;
using Zenject;

namespace Analytics
{
    public class SceneLifetimeEventLogger : IInitializable, IDisposable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private string _sceneName;

        public void Initialize()
        {
            _stopwatch.Start();
            _sceneName = SceneManager.GetActiveScene().name;
            FirebaseAnalytics.LogEvent(AnalyticEvents.SceneLoaded,
                new Parameter(AnalyticParameters.Name, _sceneName));
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            FirebaseAnalytics.LogEvent(AnalyticEvents.SceneUnloaded,
                new Parameter(AnalyticParameters.Name, _sceneName),
                new Parameter(AnalyticParameters.Time, _stopwatch.Elapsed.TotalSeconds));
        }
    }
}