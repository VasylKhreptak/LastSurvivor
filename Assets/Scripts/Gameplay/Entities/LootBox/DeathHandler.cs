using Gameplay.Entities.Health.Core;
using UnityEngine;

namespace Gameplay.Entities.LootBox
{
    public class DeathHandler : Health.DeathHandler
    {
        private readonly GameObject _gameObject;

        public DeathHandler(GameObject gameObject, IHealth health) : base(health)
        {
            _gameObject = gameObject;
        }

        protected override void OnDied()
        {
            Object.Destroy(_gameObject);
        }
    }
}