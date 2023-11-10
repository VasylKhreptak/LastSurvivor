using System;
using UnityEngine;

namespace Data.Static.Balance.Upgrade
{
    [Serializable]
    public class HelicopterPlatformUpgradePreferences
    {
        [Header("Preferences")]
        [SerializeField] private int _fuelCapacityEachLevel = 2;
        [SerializeField] private int _fuelCapacityEachUpgrade = 1;
        [SerializeField] private int _incomeMultiplierEachLevel = 1;
        [SerializeField] private float _incomeMultiplierEachUpgrade = 0.1f;
        [SerializeField] private int _upgradeCostEachLevel = 1;
        [SerializeField] private int _upgradeCostEachUpgrade = 2;

        public int FuelCapacityEachLevel => _fuelCapacityEachLevel;
        public int FuelCapacityEachUpgrade => _fuelCapacityEachUpgrade;
        public int IncomeMultiplierEachLevel => _incomeMultiplierEachLevel;
        public float IncomeMultiplierEachUpgrade => _incomeMultiplierEachUpgrade;
        public int UpgradeCostEachLevel => _upgradeCostEachLevel;
        public int UpgradeCostEachUpgrade => _upgradeCostEachUpgrade;
    }
}