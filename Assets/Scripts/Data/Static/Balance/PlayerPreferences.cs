using System;
using UnityEngine;

namespace Data.Static.Balance
{
    [Serializable]
    public class PlayerPreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private string _speedParameterName = "Speed";

        public float MovementSpeed => _movementSpeed;
        public string SpeedParameterName => _speedParameterName;
    }
}