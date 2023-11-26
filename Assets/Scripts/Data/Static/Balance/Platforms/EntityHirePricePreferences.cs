using System;
using UnityEngine;

namespace Data.Static.Balance.Platforms
{
    [Serializable]
    public class EntityHirePricePreferences
    {
        [Header("Preferences")]
        [SerializeField] private int _basePrice = 100;
        [SerializeField] private int _priceIncrement = 20;

        public int BasePrice => _basePrice;
        public int PriceIncrement => _priceIncrement;
    }
}