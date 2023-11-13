using System;
using Data.Persistent;
using Data.Persistent.Platforms;
using Data.Static.Balance.Upgrade;
using UnityEngine;
using Zenject;

namespace Platforms.OilPlatform
{
    public class OilPlatformUpgrader : IInitializable, IDisposable
    {
        private readonly ReceiveZone _receiveZone;
        private readonly OilPlatformData _platformData;
        private readonly OilPlatformUpgradePreferences _upgradePreferences;

        public OilPlatformUpgrader(ReceiveZone receiveZone, OilPlatformData platformData,
            OilPlatformUpgradePreferences upgradePreferences)
        {
            _receiveZone = receiveZone;
            _platformData = platformData;
            _upgradePreferences = upgradePreferences;
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
        }

        private void TryReduceProduceTime()
        {
            float produceTime = _platformData.BarrelProduceDuration.Value;
            produceTime -= produceTime * _upgradePreferences.ProduceTimePercentageReduce;
            produceTime = Mathf.Max(produceTime, _upgradePreferences.MinProduceTime);

            _platformData.BarrelProduceDuration.Value = produceTime;
        }

        private void TryIncreaseUpgradeCost()
        {
            if (_platformData.Level.Value % _upgradePreferences.UpgradeCostEachLevel == 0)
            {
                int upgradeCost = _platformData.UpgradeContainer.MaxValue.Value + _upgradePreferences.CostUpgradeAmount;

                _platformData.UpgradeContainer.SetMaxValue(upgradeCost);
            }
        }

        private void TryIncreaseFuelCapacity()
        {
            if (_platformData.Level.Value % _upgradePreferences.UpgradeFuelCapacityEachLevel == 0)
            {
                int fuelCapacity = _platformData.GridData.MaxValue.Value + _upgradePreferences.FuelCapacityUpgradeAmount;
                fuelCapacity = Mathf.Min(fuelCapacity, _upgradePreferences.MaxFuelCapacity);

                _platformData.GridData.SetMaxValue(fuelCapacity);
            }
        }
    }
}