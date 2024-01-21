using Gameplay.Entities.Health.DamageReceivers.Core;
using Gameplay.Entities.Health.Damages;
using Visitor;

namespace Gameplay.Entities.Health.DamageReceivers
{
    public class ExplosionDamageReceiver : BaseDamageReceiver, IVisitable<ExplosionDamage>
    {
        public void Accept(ExplosionDamage damage) => Health.TakeDamage(damage.Value);
    }
}