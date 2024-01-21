using Entities;
using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Zombie.DeathHandlers.Core
{
    public class ZombieDeathHandler : DeathHandler
    {
        private readonly Ragdoll _ragdoll;
        private readonly MonoKernel _kernel;

        public ZombieDeathHandler(Ragdoll ragdoll, MonoKernel kernel, IHealth health) : base(health)
        {
            _ragdoll = ragdoll;
            _kernel = kernel;
        }

        protected override void OnDied()
        {
            Object.Destroy(_kernel);
            _ragdoll.Activate();
        }
    }
}