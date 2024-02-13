using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using Infrastructure.Services.Vibration.Core;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Gameplay.Entities.LootBox
{
    public class LootBoxDeathHandler : DeathHandler
    {
        private readonly GameObject _gameObject;
        private readonly LootSpawner _lootSpawner;
        private readonly IVibrationService _vibrationService;

        public LootBoxDeathHandler(GameObject gameObject, LootSpawner lootSpawner, IVibrationService vibrationService,
            IHealth health) : base(health)
        {
            _gameObject = gameObject;
            _lootSpawner = lootSpawner;
            _vibrationService = vibrationService;
        }

        protected override void OnDied()
        {
            _lootSpawner.Spawn();
            _vibrationService.Vibrate(HapticPatterns.PresetType.MediumImpact);
            Object.Destroy(_gameObject);
        }
    }
}