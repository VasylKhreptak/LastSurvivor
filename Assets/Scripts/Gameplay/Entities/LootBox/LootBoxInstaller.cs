using System.Collections.Generic;
using Gameplay.Entities.Health.Core;
using Gameplay.Levels;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Utilities.CameraUtilities.Shaker;
using Zenject;

namespace Gameplay.Entities.LootBox
{
    public class LootBoxInstaller : MonoInstaller
    {
        [SerializeField] private List<Transform> _collectPoints;
        [SerializeField] private ShakeLayer.Preferences _hitShakePreferences;
        [SerializeField] private LootSpawner.Preferences _lootSpawnerPreferences;

        private LevelManager _levelManager;
        private IStaticDataService _staticDataService;

        [Inject]
        private void Constructor(LevelManager levelManager, IStaticDataService staticDataService)
        {
            _levelManager = levelManager;
            _staticDataService = staticDataService;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(transform).AsSingle();
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(GetHealth())).AsSingle();
            Container.BindInstance(_collectPoints).AsSingle();

            BindLootSpawner();
            BindShakeLayer();
            BindDeathHandler();
        }

        private float GetHealth() => _staticDataService.Balance.LootBoxHealth.Get(_levelManager.GetCurrentLevel());

        private void BindLootSpawner() => Container.Bind<LootSpawner>().AsSingle().WithArguments(_lootSpawnerPreferences);

        private void BindShakeLayer() =>
            Container.BindInterfacesAndSelfTo<ShakeLayer>().AsSingle().WithArguments(_hitShakePreferences);

        private void BindDeathHandler() => Container.BindInterfacesTo<LootBoxDeathHandler>().AsSingle();
    }
}