using UnityEngine;

namespace Gameplay.Weapons.Bullets.Core
{
    public interface IBullet
    {
        public Rigidbody Rigidbody { get; }
        public float Damage { get; }
    }
}