using Gameplay.Entities.Health.DamageHandlers.Core;
using Gameplay.Entities.Health.Damages;
using Visitor;

namespace Gameplay.Entities.Health.DamageHandlers
{
    public class ExplosionDamageHandler : BaseDamageHandler, IVisitable<ExplosionDamage>
    {
        public void Accept(ExplosionDamage damage) => _health.TakeDamage(damage.Value);
    }
}