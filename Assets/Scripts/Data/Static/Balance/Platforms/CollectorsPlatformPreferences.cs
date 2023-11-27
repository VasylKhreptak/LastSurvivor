using System;
using UnityEngine;

namespace Data.Static.Balance.Platforms
{
    [Serializable]
    public class CollectorsPlatformPreferences
    {
        [SerializeField] private EntityHirePricePreferences _collectorsHirePreferences;

        public EntityHirePricePreferences CollectorsHirePreferences => _collectorsHirePreferences;
    }
}