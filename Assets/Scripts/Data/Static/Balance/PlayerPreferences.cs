using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.Static.Balance
{
    [Serializable]
    public class PlayerPreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _velocity = 5f;
        [SerializeField] private float _rotateSpeed = 1f;
        [SerializeField] private string _speedParameterName = "Speed";

        public float Velocity => _velocity;
        public float RotateSpeed => _rotateSpeed;
        public string SpeedParameterName => _speedParameterName;
    }
}