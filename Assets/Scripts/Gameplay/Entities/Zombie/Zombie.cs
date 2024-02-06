using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    [DisallowMultipleComponent]
    public class Zombie : MonoBehaviour, IVisitable<BulletDamage>, IVisitable<ExplosionDamage>
    {
        public ZombieTargetFollower TargetFollower { get; private set; }
        public ZombieAttacker Attacker { get; private set; }
        private IHealth _health;

        [Inject]
        private void Constructor(ZombieTargetFollower targetFollower, ZombieAttacker attacker, IHealth health)
        {
            TargetFollower = targetFollower;
            Attacker = attacker;
            _health = health;
        }

        public void Accept(BulletDamage visitor) => _health.TakeDamage(visitor.Value);

        public void Accept(ExplosionDamage visitor) => _health.TakeDamage(visitor.Value);
    }
}