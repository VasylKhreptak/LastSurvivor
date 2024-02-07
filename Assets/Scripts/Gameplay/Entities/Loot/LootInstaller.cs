using Tags.Gameplay;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;
using Utilities.TransformUtilities;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Gameplay.Entities.Loot
{
    public class LootInstaller : MonoInstaller
    {
        [SerializeField] private Collider _collectTrigger;
        [SerializeField] private ClosestTriggerObserver<LootCollector>.Preferences _closestCollectorObserverPreferences;
        [SerializeField] private LootAttractor.Preferences _lootAttractorPreferences;
        [SerializeField] private TransformRotator.Preferences _transformRotatorPreferences;

        public override void InstallBindings()
        {
            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Rigidbody>().FromComponentOnRoot().AsSingle();
            Container.BindInstance(GetComponent<Variety.Core.Loot>()).AsSingle();
            Container.Bind<LootData>().AsSingle();
            Container.Bind<ToggleableManager>().AsSingle();

            BindCollectorsTriggerZone();
            BindClosestCollectorObserver();
            BindCollectHandler();
            BindLootAttractor();
            BindTransformRotator();
        }

        private void BindCollectorsTriggerZone() =>
            Container.BindInterfacesAndSelfTo<TriggerZone<LootCollector>>().AsSingle().WithArguments(_collectTrigger);

        private void BindClosestCollectorObserver() =>
            Container.BindInterfacesAndSelfTo<ClosestTriggerObserver<LootCollector>>()
                .AsSingle()
                .WithArguments(_closestCollectorObserverPreferences);

        private void BindCollectHandler() => Container.Bind<CollectHandler>().AsSingle();

        private void BindLootAttractor() =>
            Container.BindInterfacesTo<LootAttractor>().AsSingle().WithArguments(_lootAttractorPreferences);

        private void BindTransformRotator() =>
            Container.BindInterfacesTo<TransformRotator>().AsSingle().WithArguments(_transformRotatorPreferences);
    }
}