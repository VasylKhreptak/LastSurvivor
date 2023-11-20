using System;
using UnityEngine;

namespace Data.Static.Balance
{
    [Serializable]
    public class DumpWorkerPreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _minStartDelay = 0.5f;
        [SerializeField] private float _maxStartDelay = 3f;
        [SerializeField] private float _minGearSpawnInterval = 1.5f;
        [SerializeField] private float _maxGearSpawnInterval = 3;

        public float MinStartDelay => _minStartDelay;
        public float MaxStartDelay => _maxStartDelay;
        public float MinGearSpawnInterval => _minGearSpawnInterval;
        public float MaxGearSpawnInterval => _maxGearSpawnInterval;
    }
}