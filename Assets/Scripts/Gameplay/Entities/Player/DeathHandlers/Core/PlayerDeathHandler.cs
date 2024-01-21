using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using UnityEngine;

namespace Gameplay.Entities.Player.DeathHandlers.Core
{
    public class PlayerDeathHandler : DeathHandler
    {
        public PlayerDeathHandler(IHealth health) : base(health) { }

        protected override void OnDied()
        {
            Debug.Log("Player Died!");
        }
    }
}