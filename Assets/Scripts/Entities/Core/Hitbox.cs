using Entities.Core.Health.Core;
using Gameplay.Entities.Explosive.Data;
using Gameplay.Weapons.Bullets.Core;
using UnityEngine;
using Visitor;
using Zenject;

namespace Entities.Core
{
    public class Hitbox : MonoBehaviour, IVisitable<IBullet>, IVisitable<ExplosionDamage>
    {
        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health)
        {
            _health = health;
        }

        public void Accept(IBullet visitor) => _health.TakeDamage(visitor.Damage.Value);

        public void Accept(ExplosionDamage visitor) => _health.TakeDamage(visitor.Value);
    }
}