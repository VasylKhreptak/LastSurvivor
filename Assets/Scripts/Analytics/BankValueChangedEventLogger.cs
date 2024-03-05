using System;
using Firebase.Analytics;
using Plugins.Banks.Core;
using UniRx;
using Zenject;

namespace Analytics
{
    public class BankValueChangedEventLogger : IInitializable, IDisposable
    {
        private const float ThrottleTime = 0.5f;

        private readonly Bank<int> _bank;
        private readonly string _currencyName;

        public BankValueChangedEventLogger(Bank<int> bank, string currencyName)
        {
            _bank = bank;
            _currencyName = currencyName;
        }

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _bank.Value
                .Skip(1)
                .Throttle(TimeSpan.FromSeconds(ThrottleTime))
                .Subscribe(amount =>
                {
                    FirebaseAnalytics.LogEvent(AnalyticEvents.CurrencyChanged,
                        new Parameter(AnalyticParameters.Name, _currencyName),
                        new Parameter(AnalyticParameters.Amount, amount));
                });
        }

        public void Dispose() => _subscription?.Dispose();
    }
}