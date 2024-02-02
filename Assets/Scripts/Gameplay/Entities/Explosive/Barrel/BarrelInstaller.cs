using Audio.Players;
using Gameplay.Entities.Explosive.Core;
using Gameplay.Entities.Health.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Explosive.Barrel
{
    public class BarrelInstaller : MonoInstaller
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private ExplosionRigidbodyAffector.Preferences _rigidbodyAffectorPreferences;
        [SerializeField] private ExplosionDamageApplier.Preferences _explosionDamageApplierPreferences;
        [SerializeField] private AudioPlayer.Preferences _explosionAudioPlayerPreferences;
        [SerializeField] private BarrelFireBehaviour.Preferences _barrelFirePreferences;

        public override void InstallBindings()
        {
            Container.Bind<IHealth>().FromInstance(new Health.Health(_maxHealth)).AsSingle();
            Container.BindInstance(gameObject).AsSingle();

            Container.BindInterfacesTo<BarrelFireBehaviour>().AsSingle().WithArguments(_barrelFirePreferences);

            Container.Bind<ExplosionRigidbodyAffector>().AsSingle().WithArguments(_rigidbodyAffectorPreferences);
            Container.Bind<ExplosionDamageApplier>().AsSingle().WithArguments(_explosionDamageApplierPreferences);
            Container.Bind<AudioPlayer>().AsSingle().WithArguments(_explosionAudioPlayerPreferences);
            Container.BindInterfacesTo<BarrelExploder>().AsSingle();
        }
    }
}