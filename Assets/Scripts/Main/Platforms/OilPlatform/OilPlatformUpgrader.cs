using System;
using Analytics;
using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using Firebase.Analytics;
using Main.Platforms.Zones;
using UnityEngine;
using Zenject;

namespace Main.Platforms.OilPlatform
{
    public class OilPlatformUpgrader : IInitializable, IDisposable
    {
        private readonly ReceiveZone _receiveZone;
        private readonly OilPlatformData _platformData;
        private readonly OilPlatformPreferences _preferences;

        public OilPlatformUpgrader(ReceiveZone receiveZone, OilPlatformData platformData,
            OilPlatformPreferences preferences)
        {
            _receiveZone = receiveZone;
            _platformData = platformData;
            _preferences = preferences;
        }

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving() => _receiveZone.OnReceivedAll += Upgrade;

        private void StopObserving() => _receiveZone.OnReceivedAll -= Upgrade;

        private void Upgrade()
        {
            _platformData.Level.Value++;

            TryReduceProduceTime();
            TryIncreaseUpgradeCost();
            TryIncreaseFuelCapacity();

            LogEvent();
        }

        private void TryReduceProduceTime()
        {
            float produceTime = _platformData.BarrelProduceDuration.Value;
            produceTime -= produceTime * _preferences.ProduceTimePercentageReduce;
            produceTime = Mathf.Max(produceTime, _preferences.MinProduceTime);

            _platformData.BarrelProduceDuration.Value = produceTime;
        }

        private void TryIncreaseUpgradeCost()
        {
            if (_platformData.Level.Value % _preferences.UpgradeCostEachLevel == 0)
            {
                int upgradeCost = _platformData.UpgradeContainer.MaxValue.Value + _preferences.CostUpgradeAmount;

                _platformData.UpgradeContainer.SetMaxValue(upgradeCost);
            }
        }

        private void TryIncreaseFuelCapacity()
        {
            if (_platformData.Level.Value % _preferences.UpgradeFuelCapacityEachLevel == 0)
            {
                int fuelCapacity = _platformData.GridData.MaxValue.Value + _preferences.FuelCapacityUpgradeAmount;
                fuelCapacity = Mathf.Min(fuelCapacity, _preferences.MaxFuelCapacity);

                _platformData.GridData.SetMaxValue(fuelCapacity);
            }
        }

        private void LogEvent()
        {
            FirebaseAnalytics.LogEvent(AnalyticEvents.UpgradedPlatform,
                new Parameter(AnalyticParameters.Name, "Oil Platform"),
                new Parameter("Level", _platformData.Level.Value),
                new Parameter("Product time", _platformData.BarrelProduceDuration.Value),
                new Parameter("Upgrade Cost", _platformData.UpgradeContainer.MaxValue.Value),
                new Parameter("Fuel Capacity", _platformData.GridData.MaxValue.Value));
        }
    }
}