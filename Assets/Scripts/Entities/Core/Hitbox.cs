using Entities.Core.Health.Core;
using Gameplay.Weapons.Bullets.Core;
using UnityEngine;
using Visitor;
using Zenject;

namespace Entities.Core
{
    public class Hitbox : MonoBehaviour, IVisitable<IBullet>
    {
        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health)
        {
            _health = health;
        }

        public void Accept(IBullet visitor) => _health.TakeDamage(visitor.Damage);
    }
}