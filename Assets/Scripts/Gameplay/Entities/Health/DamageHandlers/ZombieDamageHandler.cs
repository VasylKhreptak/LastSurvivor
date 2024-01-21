using Gameplay.Entities.Health.DamageHandlers.Core;
using Gameplay.Entities.Health.Damages;
using Visitor;

namespace Gameplay.Entities.Health.DamageHandlers
{
    public class ZombieDamageHandler : BaseDamageHandler, IVisitable<ZombieDamage>
    {
        public void Accept(ZombieDamage damage) => _health.TakeDamage(damage.Value);
    }
}