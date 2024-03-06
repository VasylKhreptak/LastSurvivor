using System;
using System.Diagnostics;
using Firebase.Analytics;
using Observers;
using UniRx;
using Zenject;

namespace Analytics
{
    public class IdleEventLogger : IInitializable, IDisposable
    {
        private readonly IdleObserver _idleObserver;

        public IdleEventLogger(IdleObserver idleObserver)
        {
            _idleObserver = idleObserver;
        }

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private IDisposable _idleSubscription;

        public void Initialize() => _idleSubscription = _idleObserver.IsIdling.Skip(1).Subscribe(OnIsIdlingValueChanged);

        public void Dispose()
        {
            _idleSubscription?.Dispose();
            _stopwatch.Stop();
        }

        private void OnIsIdlingValueChanged(bool isIdling)
        {
            if (isIdling)
            {
                _stopwatch.Start();
                FirebaseAnalytics.LogEvent(AnalyticEvents.StartedIdling);
            }
            else
            {
                double idleDuration = _stopwatch.Elapsed.TotalSeconds;
                FirebaseAnalytics.LogEvent(AnalyticEvents.StoppedIdling, new Parameter(AnalyticParameters.Duration, idleDuration));
                _stopwatch.Restart();
            }
        }
    }
}