using System;
using Data.Persistent;
using Data.Static.Balance.Upgrade;
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

            ReduceProduceTime();
            TryIncreaseUpgradeCost();
        }

        private void ReduceProduceTime()
        {
            _platformData.BarrelProduceDuration.Value -=
                _platformData.BarrelProduceDuration.Value * _upgradePreferences.ProduceTimePercentageReduce;
        }

        private void TryIncreaseUpgradeCost()
        {
            if (_platformData.Level.Value % _upgradePreferences.UpgradeCostEachLevel == 0)
            {
                int upgradeCost = _platformData.UpgradeContainer.MaxValue.Value +
                                  _upgradePreferences.UpgradeCostEachUpgrade;

                _platformData.UpgradeContainer.SetMaxValue(upgradeCost);
            }
        }
    }
}