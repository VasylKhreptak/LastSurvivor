using Entities.Health;
using UnityEngine;

namespace Gameplay.Weapons.Bullets.Core
{
    public interface IBullet
    {
        public Transform Transform { get; }
        public Rigidbody Rigidbody { get; }
        public Damage Damage { get; }
    }
}