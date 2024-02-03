using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using UnityEngine;

namespace Gameplay.Entities.LootBox
{
    public class LootBoxDeathHandler : DeathHandler
    {
        private readonly GameObject _gameObject;

        public LootBoxDeathHandler(GameObject gameObject, IHealth health) : base(health)
        {
            _gameObject = gameObject;
        }

        protected override void OnDied()
        {
            Object.Destroy(_gameObject);
        }
    }
}