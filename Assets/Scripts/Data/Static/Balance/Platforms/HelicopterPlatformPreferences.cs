using System;
using UnityEngine;

namespace Data.Static.Balance.Platforms
{
    [Serializable]
    public class HelicopterPlatformPreferences
    {
        [Header("Preferences")]
        [SerializeField] private int _upgradeFuelCapacityEachLevel = 2;
        [SerializeField] private int _fuelCapacityUpgradeAmount = 1;
        [SerializeField] private int _upgradeIncomeMultiplierEachLevel = 1;
        [SerializeField] private float _incomeMultiplierUpgradeAmount = 0.1f;
        [SerializeField] private int _upgradeCostEachLevel = 1;
        [SerializeField] private int _costUpgradeAmount = 2;
        [SerializeField] private int _minigunAmmoCapacityUpgradeAmount = 4;
        [SerializeField] private int _maxMinigunAmmoCapacity = 300;

        public int UpgradeFuelCapacityEachLevel => _upgradeFuelCapacityEachLevel;
        public int FuelCapacityUpgradeAmount => _fuelCapacityUpgradeAmount;
        public int UpgradeIncomeMultiplierEachLevel => _upgradeIncomeMultiplierEachLevel;
        public float IncomeMultiplierUpgradeAmount => _incomeMultiplierUpgradeAmount;
        public int UpgradeCostEachLevel => _upgradeCostEachLevel;
        public int CostUpgradeAmount => _costUpgradeAmount;
        public int MinigunAmmoCapacityUpgradeAmount => _minigunAmmoCapacityUpgradeAmount;
        public int MaxMinigunAmmoCapacity => _maxMinigunAmmoCapacity;
    }
}