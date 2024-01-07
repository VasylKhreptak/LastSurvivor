using UnityEngine;

namespace Entities.Core.Health
{
    public class Damage
    {
        private float _value;

        public Damage(float value)
        {
            Value = value;
        }

        public float Value
        {
            get => _value;
            set => _value = Mathf.Max(0, value);
        }
    }
}