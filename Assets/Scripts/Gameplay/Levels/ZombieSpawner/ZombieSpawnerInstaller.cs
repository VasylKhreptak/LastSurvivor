using System.Collections.Generic;
using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;
using Visitor;
using Zenject;

namespace Gameplay.Levels.ZombieSpawner
{
    public class ZombieSpawnerInstaller : MonoInstaller
    {
        [SerializeField] private Collider _trigger;
        [SerializeField] private ZombieSpawner.Preferences _zombieSpawnerPreferences;
        [SerializeField] private ClosestTriggerObserver<IVisitable<ZombieDamage>>.Preferences _closestTargetObserverPreferences;

        private List<ZombieSpawner> _zombieSpawners;

        [Inject]
        private void Constructor(List<ZombieSpawner> zombieSpawners) => _zombieSpawners = zombieSpawners;

        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).AsSingle();
            Container.BindInstance(transform).AsSingle();
            Container.BindInstance(_trigger).AsSingle();

            BindTargetsZone();
            BindClosestTargetObserver();
            BindZombieSpawner();
            RegisterZombieSpawner();
        }

        private void BindTargetsZone() => Container.BindInterfacesAndSelfTo<TriggerZone<IVisitable<ZombieDamage>>>().AsSingle();

        private void BindClosestTargetObserver()
        {
            Container
                .BindInterfacesAndSelfTo<ClosestTriggerObserver<IVisitable<ZombieDamage>>>()
                .AsSingle()
                .WithArguments(_closestTargetObserverPreferences);
        }

        private void BindZombieSpawner() =>
            Container.BindInterfacesAndSelfTo<ZombieSpawner>().AsSingle().WithArguments(_zombieSpawnerPreferences);

        private void RegisterZombieSpawner() => _zombieSpawners.Add(Container.Resolve<ZombieSpawner>());
    }
}