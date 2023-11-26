using System;
using UnityEngine;

namespace Data.Static.Balance.Platforms
{
    [Serializable]
    public class OilPlatformPreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _produceTimePercentageReduce = 0.1f;
        [SerializeField] private float _minProduceTime = 0.3f;
        [SerializeField] private int _upgradeCostEachLevel = 1;
        [SerializeField] private int _costUpgradeAmount = 2;
        [SerializeField] private int _upgradeFuelCapacityEachLevel = 3;
        [SerializeField] private int _fuelCapacityUpgradeAmount = 1;
        [SerializeField] private int _maxFuelCapacity = 18;

        public float ProduceTimePercentageReduce => _produceTimePercentageReduce;
        public float MinProduceTime => _minProduceTime;
        public int UpgradeCostEachLevel => _upgradeCostEachLevel;
        public int CostUpgradeAmount => _costUpgradeAmount;
        public int UpgradeFuelCapacityEachLevel => _upgradeFuelCapacityEachLevel;
        public int FuelCapacityUpgradeAmount => _fuelCapacityUpgradeAmount;
        public int MaxFuelCapacity => _maxFuelCapacity;
    }
}