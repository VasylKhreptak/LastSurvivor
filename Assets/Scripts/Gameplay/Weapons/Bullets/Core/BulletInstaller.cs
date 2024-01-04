using Gameplay.Weapons.Bullets.CollisionHandlers;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Bullets.Core
{
    public class BulletInstaller : MonoInstaller
    {
        [Header("Preferences")]
        [SerializeField] private HitParticle.Preferences _hitParticlePreferences;

        public override void InstallBindings()
        {
            Container.BindInstance(GetComponent<IBullet>()).AsSingle();
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Collider>().FromComponentOnRoot().AsSingle();
            Container.Bind<TrailRenderer>().FromComponentOnRoot().AsSingle();

            BindCollisionHandlers();
        }

        private void BindCollisionHandlers()
        {
            Container.Bind<TrailReseter>().AsSingle();
            Container.Bind<HitParticle>().AsSingle().WithArguments(_hitParticlePreferences);

            Container.BindInterfacesTo<CollisionHandler>().AsSingle();
        }
    }
}