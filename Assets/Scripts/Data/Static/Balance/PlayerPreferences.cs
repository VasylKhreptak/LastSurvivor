using System;
using UnityEngine;

namespace Data.Static.Balance
{
    [Serializable]
    public class PlayerPreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _acceleration = 1f;
        [SerializeField] private float _rotateSpeed = 1f;
        [SerializeField] private float _gravity = 1f;
        [SerializeField] private string _speedParameterName = "Speed";

        public float Acceleration => _acceleration;
        public float RotateSpeed => _rotateSpeed;
        public float Gravity => _gravity;
        public string SpeedParameterName => _speedParameterName;
    }
}