using System;
using System.Diagnostics;
using Firebase.Analytics;
using Zenject;

namespace Analytics
{
    public class ApplicationLifetimeEventLogger : IInitializable, IDisposable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public void Initialize()
        {
            _stopwatch.Start();
            FirebaseAnalytics.LogEvent(AnalyticEvents.ApplicationOpen);
        }

        public void Dispose()
        {
            FirebaseAnalytics.LogEvent(AnalyticEvents.ApplicationClose, AnalyticParameters.Duration, _stopwatch.Elapsed.TotalSeconds);
            _stopwatch.Stop();
        }
    }
}