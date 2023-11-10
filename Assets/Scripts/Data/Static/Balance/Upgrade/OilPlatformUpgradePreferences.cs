using System;
using UnityEngine;

namespace Data.Static.Balance.Upgrade
{
    [Serializable]
    public class OilPlatformUpgradePreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _produceTimePercentageReduce = 0.1f;
        [SerializeField] private int _upgradeCostEachLevel = 1;
        [SerializeField] private int _upgradeCostEachUpgrade = 2;

        public float ProduceTimePercentageReduce => _produceTimePercentageReduce;
        public int UpgradeCostEachLevel => _upgradeCostEachLevel;
        public int UpgradeCostEachUpgrade => _upgradeCostEachUpgrade;
    }
}