using System;
using UnityEngine;

namespace Data.Static.Balance
{
    [Serializable]
    public class TransformLookerPreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _defaultLookSpeed = 1f;
        [SerializeField] private Vector3 _rotationOffsetForUI = new Vector3(0f, 180f, 0f);

        public float DefaultLookSpeed => _defaultLookSpeed;
        public Vector3 RotationOffsetForUI => _rotationOffsetForUI;
    }
}