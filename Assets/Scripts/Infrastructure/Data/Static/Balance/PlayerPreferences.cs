using System;
using UnityEngine;

namespace Infrastructure.Data.Static.Balance
{
    [Serializable]
    public class PlayerPreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _movementSpeed = 5f;

        public float MovementSpeed => _movementSpeed;
    }
}