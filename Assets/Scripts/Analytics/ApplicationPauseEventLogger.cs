using System;
using Firebase.Analytics;
using UniRx;
using Zenject;

namespace Analytics
{
    public class ApplicationPauseEventLogger : IInitializable, IDisposable
    {
        private IDisposable _subscription;

        public void Initialize() => _subscription = Observable.EveryApplicationPause().Subscribe(OnApplicationPause);

        public void Dispose() => _subscription?.Dispose();

        private void OnApplicationPause(bool pauseStatus) =>
            FirebaseAnalytics.LogEvent(pauseStatus ? AnalyticEvents.ApplicationPaused : AnalyticEvents.ApplicationResumed);
    }
}