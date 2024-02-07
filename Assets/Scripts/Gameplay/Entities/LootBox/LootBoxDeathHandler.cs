using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using UnityEngine;

namespace Gameplay.Entities.LootBox
{
    public class LootBoxDeathHandler : DeathHandler
    {
        private readonly GameObject _gameObject;
        private readonly LootSpawner _lootSpawner;

        public LootBoxDeathHandler(GameObject gameObject, LootSpawner lootSpawner, IHealth health) : base(health)
        {
            _gameObject = gameObject;
            _lootSpawner = lootSpawner;
        }

        protected override void OnDied()
        {
            _lootSpawner.Spawn();
            Object.Destroy(_gameObject);
        }
    }
}