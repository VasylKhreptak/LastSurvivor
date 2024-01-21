using Gameplay.Entities.Health.DamageHandlers.Core;
using Gameplay.Entities.Health.Damages;
using Visitor;

namespace Gameplay.Entities.Health.DamageHandlers
{
    public class BulletDamageHandler : BaseDamageHandler, IVisitable<BulletDamage>
    {
        public void Accept(BulletDamage damage) => _health.TakeDamage(damage.Value);
    }
}