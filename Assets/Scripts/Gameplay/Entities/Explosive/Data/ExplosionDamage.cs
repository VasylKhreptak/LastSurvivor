using UnityEngine;

namespace Gameplay.Entities.Explosive.Data
{
    public class ExplosionDamage
    {
        public readonly float Value;

        public ExplosionDamage(float value)
        {
            Value = Mathf.Max(0, value);
        }
    }
}