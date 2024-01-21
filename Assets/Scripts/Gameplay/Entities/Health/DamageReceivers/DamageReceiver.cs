using Gameplay.Entities.Health.DamageReceivers.Core;
using Gameplay.Entities.Health.Damages.Core;
using Visitor;

namespace Gameplay.Entities.Health.DamageReceivers
{
    public class DamageReceiver : BaseDamageReceiver, IVisitable<Damage>
    {
        public void Accept(Damage damage) => Health.TakeDamage(damage.Value);
    }
}