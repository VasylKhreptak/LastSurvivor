using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Utilities.CameraUtilities.Shaker;
using Zenject;

namespace Gameplay.Entities.LootBox
{
    public class LootBoxInstaller : MonoInstaller
    {
        [SerializeField] private ShakeLayer.Preferences _hitShakePreferences;

        private IPersistentDataService _persistentDataService;
        private IStaticDataService _staticDataService;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _persistentDataService = persistentDataService;
            _staticDataService = staticDataService;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(transform).AsSingle();
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(GetHealth())).AsSingle();

            BindHitShakeLayer();
            BindDamageShaker();
            BIndDeathHandler();
        }

        private float GetHealth() =>
            _staticDataService.Balance.LootBoxHealth.Get(_persistentDataService.PersistentData.PlayerData.Level);

        private void BIndDeathHandler() => Container.BindInterfacesTo<LootBoxDeathHandler>().AsSingle();

        private void BindHitShakeLayer() =>
            Container.BindInterfacesAndSelfTo<ShakeLayer>().AsSingle().WithArguments(_hitShakePreferences);

        private void BindDamageShaker() => Container.BindInterfacesTo<DamageShaker>().AsSingle();
    }
}