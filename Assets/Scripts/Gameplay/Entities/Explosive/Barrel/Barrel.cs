using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Explosive.Barrel
{
    public class Barrel : MonoBehaviour, IVisitable<BulletDamage>, IVisitable<ExplosionDamage>
    {
        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health)
        {
            _health = health;
        }

        public void Accept(BulletDamage visitor) => _health.TakeDamage(visitor.Value);

        public void Accept(ExplosionDamage visitor) => _health.TakeDamage(visitor.Value);
    }
}