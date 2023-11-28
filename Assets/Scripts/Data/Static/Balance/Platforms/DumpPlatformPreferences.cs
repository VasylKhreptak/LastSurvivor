using System;
using UnityEngine;

namespace Data.Static.Balance.Platforms
{
    [Serializable]
    public class DumpPlatformPreferences
    {
        [Header("Worker Start Preferences")]
        [SerializeField] private float _minWorkerStartDelay = 0.5f;
        [SerializeField] private float _maxWorkerStartDelay = 3f;

        [Header("Gear Spawn Preferences")]
        [SerializeField] private float _minGearSpawnInterval = 1.5f;
        [SerializeField] private float _maxGearSpawnInterval = 3;

        [Header("Worker Tool Animation Preferences")]
        [SerializeField] private float _workerToolAnimationDuration = 0.5f;
        [SerializeField] private AnimationCurve _workerToolAnimationCurve;

        [Header("Hire Preferences")]
        [SerializeField] private EntityHirePricePreferences _workerHirePricePreferences;

        public float MinWorkerStartDelay => _minWorkerStartDelay;
        public float MaxWorkerStartDelay => _maxWorkerStartDelay;

        public float MinGearSpawnInterval => _minGearSpawnInterval;
        public float MaxGearSpawnInterval => _maxGearSpawnInterval;

        public EntityHirePricePreferences WorkerHirePricePreferences => _workerHirePricePreferences;

        public float WorkerToolAnimationDuration => _workerToolAnimationDuration;
        public AnimationCurve WorkerToolAnimationCurve => _workerToolAnimationCurve;
    }
}