using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Visitor;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class DamageApplier
    {
        private readonly BulletDamage _bulletDamage;

        public DamageApplier(BulletDamage bulletDamage) => _bulletDamage = bulletDamage;

        public void TryApply(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out IVisitable<BulletDamage> visitable))
                visitable.Accept(_bulletDamage);
        }
    }
}