using System;
using UnityEngine;

namespace Data.Static.Balance
{
    [Serializable]
    public class TransformLookerPreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _defaultLookSpeed = 1f;

        public float DefaultLookSpeed => _defaultLookSpeed;
    }
}