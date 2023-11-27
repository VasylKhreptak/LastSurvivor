using System;
using UnityEngine;

namespace Platforms.DumpPlatform.Workers
{
    [Serializable]
    public class WorkerReferences
    {
        [SerializeField] private Transform _gearSpawnPoint;
        [SerializeField] private Transform _toolTransform;

        public Transform GearSpawnPoint => _gearSpawnPoint;
        public Transform ToolTransform => _toolTransform;
    }
}