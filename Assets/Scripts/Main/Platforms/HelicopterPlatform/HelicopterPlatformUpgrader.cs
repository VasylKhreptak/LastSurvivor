﻿using System;
using Analytics;
using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using Firebase.Analytics;
using Main.Platforms.Zones;
using Zenject;

namespace Main.Platforms.HelicopterPlatform
{
    public class HelicopterPlatformUpgrader : IInitializable, IDisposable
    {
        private readonly ReceiveZone _receiveZone;
        private readonly HelicopterPlatformPreferences _platformPreferences;
        private readonly HelicopterPlatformData _platformData;

        public HelicopterPlatformUpgrader(ReceiveZone receiveZone, HelicopterPlatformPreferences platformPreferences,
            HelicopterPlatformData platformData)
        {
            _receiveZone = receiveZone;
            _platformPreferences = platformPreferences;
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
            TryIncreaseMinigunAmmoCapacity();

            LogEvent();
        }

        private void TryIncreaseTankCapacity()
        {
            bool canUpgradeTankCapacity = _platformData.Level.Value % _platformPreferences.UpgradeFuelCapacityEachLevel == 0;

            if (canUpgradeTankCapacity)
            {
                int tankCapacity = _platformData.FuelTank.MaxValue.Value +
                                   _platformPreferences.FuelCapacityUpgradeAmount;

                _platformData.FuelTank.SetMaxValue(tankCapacity);
            }
        }

        private void TryIncreaseIncomeMultiplier()
        {
            bool canUpgradeIncomeMultiplier =
                _platformData.Level.Value % _platformPreferences.UpgradeIncomeMultiplierEachLevel == 0;

            if (canUpgradeIncomeMultiplier)
                _platformData.IncomeMultiplier.Value += _platformPreferences.IncomeMultiplierUpgradeAmount;
        }

        private void TryIncreaseUpgradeCost()
        {
            bool canUpgradeUpgradeCost = _platformData.Level.Value % _platformPreferences.UpgradeCostEachLevel == 0;

            if (canUpgradeUpgradeCost)
            {
                int cost = _platformData.UpgradeContainer.MaxValue.Value + _platformPreferences.CostUpgradeAmount;

                _platformData.UpgradeContainer.SetMaxValue(cost);
            }
        }

        private void TryIncreaseMinigunAmmoCapacity()
        {
            int capacity = _platformData.MinigunAmmoCapacity + _platformPreferences.MinigunAmmoCapacityUpgradeAmount;
            capacity = Math.Min(capacity, _platformPreferences.MaxMinigunAmmoCapacity);
            _platformData.MinigunAmmoCapacity = capacity;
        }

        private void LogEvent()
        {
            FirebaseAnalytics.LogEvent(AnalyticEvents.UpgradedPlatform,
                new Parameter(AnalyticParameters.Name, "Helicopter Platform"),
                new Parameter("Level", _platformData.Level.Value),
                new Parameter("Tank capacity", _platformData.FuelTank.MaxValue.Value),
                new Parameter("Income multiplier", _platformData.IncomeMultiplier.Value),
                new Parameter("Upgrade Cost", _platformData.UpgradeContainer.MaxValue.Value),
                new Parameter("Minigun ammo", _platformData.MinigunAmmoCapacity));
        }
    }
}