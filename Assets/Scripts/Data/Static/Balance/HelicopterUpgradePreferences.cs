using System;
using UnityEngine;

namespace Data.Static.Balance
{
    [Serializable]
    public class HelicopterUpgradePreferences
    {
        [Header("Preferences")]
        [SerializeField] private int _increaseFuelCapacityEachLevel = 2;
        [SerializeField] private int _incrementFuelCapacityEachUpgrade = 1;
        [SerializeField] private float _increaseIncomeMultiplierEachLevel = 1;
        [SerializeField] private float _incrementIncomeMultiplierEachUpgrade = 0.1f;

        public int IncreaseFuelCapacityEachLevel => _increaseFuelCapacityEachLevel;
        public int IncrementFuelCapacityEachUpgrade => _incrementFuelCapacityEachUpgrade;
        public float IncreaseIncomeMultiplierEachLevel => _increaseIncomeMultiplierEachLevel;
        public float IncrementIncomeMultiplierEachUpgrade => _incrementIncomeMultiplierEachUpgrade;
    }
}