using Gameplay.Entities.Health.Damages;
using Gameplay.Weapons.Bullets.CollisionHandlers;
using Gameplay.Weapons.Bullets.CollisionHandlers.Core;
using ObjectPoolSystem;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Gameplay.Weapons.Bullets
{
    public class BulletInstaller : MonoInstaller
    {
        [Header("Preferences")]
        [SerializeField] private HitParticle.Preferences _hitParticlePreferences;
        [SerializeField] private LifetimeHandler.Preferences _lifetimePreferences;
        [SerializeField] private ImpulseTransmitter.Preferences _impulseTransmitterPreferences;
        [SerializeField] private HitAudioPlayer.Preferences _hitAudioPlayerPreferences;
        [SerializeField] private float _defaultDamage = 10f;

        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Collider>().FromComponentOnRoot().AsSingle();
            Container.Bind<TrailRenderer>().FromComponentOnRoot().AsSingle();
            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.Bind<Rigidbody>().FromComponentOnRoot().AsSingle();
            Container.Bind<BulletDamage>().AsSingle().WithArguments(_defaultDamage);

            Container.Bind<ToggleableManager>().AsSingle();
            Container.BindInterfacesTo<LifetimeHandler>().AsSingle().WithArguments(_lifetimePreferences);
            Container.BindInterfacesTo<TrailReseter>().AsSingle();

            BindCollisionHandler();
        }

        private void BindCollisionHandler()
        {
            Container.Bind<HitParticle>().AsSingle().WithArguments(_hitParticlePreferences);
            Container.Bind<HitAudioPlayer>().AsSingle().WithArguments(_hitAudioPlayerPreferences);
            Container.Bind<DamageApplier>().AsSingle();
            Container.Bind<ImpulseTransmitter>().AsSingle().WithArguments(_impulseTransmitterPreferences);
            Container.BindInterfacesTo<BulletCollisionHandler>().AsSingle();
        }
    }
}