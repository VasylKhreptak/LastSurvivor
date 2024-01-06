using Gameplay.Weapons.Bullets.CollisionHandlers;
using ObjectPoolSystem;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Gameplay.Weapons.Bullets.Core
{
    public class BulletInstaller : MonoInstaller
    {
        [Header("Preferences")]
        [SerializeField] private HitParticle.Preferences _hitParticlePreferences;
        [SerializeField] private LifetimeHandler.Preferences _lifetimePreferences;

        public override void InstallBindings()
        {
            Container.BindInstance(GetComponent<IBullet>()).AsSingle();
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Collider>().FromComponentOnRoot().AsSingle();
            Container.Bind<TrailRenderer>().FromComponentOnRoot().AsSingle();

            Container.Bind<ToggleableManager>().AsSingle();

            Container.BindInterfacesTo<LifetimeHandler>().AsSingle().WithArguments(_lifetimePreferences);
            Container.BindInterfacesTo<TrailRenderer>().AsSingle();

            Container.Bind<HitParticle>().AsSingle().WithArguments(_hitParticlePreferences);
            Container.BindInterfacesTo<CollisionHandler>().AsSingle();
        }
    }
}