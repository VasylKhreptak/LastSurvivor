using System;
using Data.Persistent;
using Data.Static.Balance.Upgrade;
using Zenject;

namespace Platforms.HelicopterPlatform
{
    public class HelicopterPlatformUpgrader : IInitializable, IDisposable
    {
        private readonly ReceiveZone _receiveZone;
        private readonly HelicopterPlatformUpgradePreferences _platformUpgradePreferences;
        private readonly HelicopterPlatformData _platformData;

        public HelicopterPlatformUpgrader(ReceiveZone receiveZone, HelicopterPlatformUpgradePreferences platformUpgradePreferences,
            HelicopterPlatformData platformData)
        {
            _receiveZone = receiveZone;
            _platformUpgradePreferences = platformUpgradePreferences;
            _platformData = platformData;
        }

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving() => _receiveZone.OnReceivedAll += Upgrade;

        private void StopObserving() => _receiveZone.OnReceivedAll -= Upgrade;

        private void Upgrade()
        {
            _platformData.Level.Value++;

            TryIncreaseTankCapacity();
            TryIncreaseIncomeMultiplier();
            TryIncreaseUpgradeCost();
        }

        private void TryIncreaseTankCapacity()
        {
            bool canUpgradeTankCapacity = _platformData.Level.Value % _platformUpgradePreferences.FuelCapacityEachLevel == 0;

            if (canUpgradeTankCapacity)
            {
                int tankCapacity = _platformData.FuelTank.MaxValue.Value +
                                   _platformUpgradePreferences.FuelCapacityEachUpgrade;

                _platformData.FuelTank.SetMaxValue(tankCapacity);
            }
        }

        private void TryIncreaseIncomeMultiplier()
        {
            bool canUpgradeIncomeMultiplier =
                _platformData.Level.Value % _platformUpgradePreferences.IncomeMultiplierEachLevel == 0;

            if (canUpgradeIncomeMultiplier)
                _platformData.IncomeMultiplier.Value += _platformUpgradePreferences.IncomeMultiplierEachUpgrade;
        }

        private void TryIncreaseUpgradeCost()
        {
            bool canUpgradeUpgradeCost = _platformData.Level.Value % _platformUpgradePreferences.UpgradeCostEachLevel == 0;

            if (canUpgradeUpgradeCost)
            {
                int cost = _platformData.UpgradeContainer.MaxValue.Value + _platformUpgradePreferences.UpgradeCostEachUpgrade;

                _platformData.UpgradeContainer.SetMaxValue(cost);
            }
        }
    }
}