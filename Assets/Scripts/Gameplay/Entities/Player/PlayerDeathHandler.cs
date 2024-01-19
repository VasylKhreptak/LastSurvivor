using Entities.Health;
using Entities.Health.Core;
using UnityEngine;

namespace Gameplay.Entities.Player
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