using Gameplay.Entities.Health.DamageReceivers.Core;
using Gameplay.Entities.Health.Damages;
using Visitor;

namespace Gameplay.Entities.Health.DamageReceivers
{
    public class BulletDamageReceiver : BaseDamageReceiver, IVisitable<BulletDamage>
    {
        public void Accept(BulletDamage damage) => Health.TakeDamage(damage.Value);
    }
}