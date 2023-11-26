using System;
using UnityEngine;

namespace Data.Static.Balance.Platforms
{
    [Serializable]
    public class BarracksPlatformPreferences
    {
        [SerializeField] private EntityHirePricePreferences _soldierHirePricePreferences;

        public EntityHirePricePreferences SoldierHirePricePreferences => _soldierHirePricePreferences;
    }
}