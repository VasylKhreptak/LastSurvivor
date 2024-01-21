using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;

namespace Gameplay.Entities.Zombie.DeathHandlers.Core
{
    public class ZombieDeathHandler : DeathHandler
    {
        public ZombieDeathHandler(IHealth health) : base(health) { }
    }
}