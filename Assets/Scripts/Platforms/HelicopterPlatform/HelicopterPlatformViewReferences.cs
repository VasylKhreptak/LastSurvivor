using System;
using UnityEngine;

namespace Platforms.HelicopterPlatform
{
    [Serializable]
    public class HelicopterPlatformViewReferences
    {
        [Header("References")]
        [SerializeField] private Collider _upgradeZoneTrigger;

        public Collider UpgradeZoneTrigger => _upgradeZoneTrigger;
    }
}