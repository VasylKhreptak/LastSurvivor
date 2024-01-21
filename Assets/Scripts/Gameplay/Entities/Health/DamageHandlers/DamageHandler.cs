using Gameplay.Entities.Health.DamageHandlers.Core;
using Gameplay.Entities.Health.Damages.Core;
using Visitor;

namespace Gameplay.Entities.Health.DamageHandlers
{
    public class DamageHandler : BaseDamageHandler, IVisitable<Damage>
    {
        public void Accept(Damage damage) => _health.TakeDamage(damage.Value);
    }
}