using Gameplay.Entities.Health.DamageReceivers.Core;
using Gameplay.Entities.Health.Damages;
using Visitor;

namespace Gameplay.Entities.Health.DamageReceivers
{
    public class ZombieDamageReceiver : BaseDamageReceiver, IVisitable<ZombieDamage>
    {
        public void Accept(ZombieDamage damage) => Health.TakeDamage(damage.Value);
    }
}