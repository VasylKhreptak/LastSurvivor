using System;
using Infrastructure.Services.PersistentData.Core;
using Zenject;

namespace Analytics
{
    public class ResourcesCountEventLogger : IInitializable, IDisposable
    {
        private readonly BankValueChangedEventLogger _moneyValueChangedEventLogger;
        private readonly BankValueChangedEventLogger _gearsValueChangedEventLogger;

        public ResourcesCountEventLogger(IPersistentDataService persistentDataService)
        {
            _moneyValueChangedEventLogger =
                new BankValueChangedEventLogger(persistentDataService.Data.PlayerData.Resources.Money, "money");
            _gearsValueChangedEventLogger =
                new BankValueChangedEventLogger(persistentDataService.Data.PlayerData.Resources.Gears, "gears");
        }

        public void Initialize()
        {
            _moneyValueChangedEventLogger.Initialize();
            _gearsValueChangedEventLogger.Initialize();
        }

        public void Dispose()
        {
            _moneyValueChangedEventLogger.Dispose();
            _gearsValueChangedEventLogger.Dispose();
        }
    }
}