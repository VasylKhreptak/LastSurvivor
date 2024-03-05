using System;
using Analytics;
using Firebase.Analytics;
using UnityEngine;
using Zenject;

namespace Main.Platforms.BuyZones.Core.Analytics
{
    public class BuyPlatformEventLogger : IInitializable, IDisposable
    {
        private readonly PlatformBuyer _platformBuyer;
        private readonly string _platformName;

        public BuyPlatformEventLogger(PlatformBuyer platformBuyer, string platformName)
        {
            _platformBuyer = platformBuyer;
            _platformName = platformName;
        }

        public void Initialize() => _platformBuyer.OnBought += OnBoughtPlatform;

        public void Dispose() => _platformBuyer.OnBought -= OnBoughtPlatform;

        private void OnBoughtPlatform(GameObject platform) => LogEvent();

        private void LogEvent() =>
            FirebaseAnalytics.LogEvent(AnalyticEvents.BoughtPlatform, new Parameter(AnalyticParameters.Name, _platformName));
    }
}