using Adapters.Velocity;
using Audio.Players;
using Data.Persistent.Platforms;
using Gameplay.Weapons.Core.Fire;
using Gameplay.Weapons.Minigun.StateMachine;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.Services.PersistentData.Core;
using Plugins.Banks;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable;

namespace Gameplay.Weapons.Minigun
{
    public class MinigunInstaller : MonoInstaller
    {
        [Header("Preferences")]
        [SerializeField] private WeaponAimer.Preferences _aimPreferences;
        [SerializeField] private BarrelSpiner.Preferences _barrelRotatorPreferences;
        [SerializeField] private ShootState.Preferences _firePreferences;
        [SerializeField] private AudioPlayer.Preferences _fireAudioPreferences;
        [SerializeField] private MinigunSpinAudio.Preferences _spinAudioPreferences;
        [SerializeField] private ShootParticle.Preferences _shootParticlePreferences;
        [SerializeField] private ShellSpawner.Preferences _shellSpawnerPreferences;

        private HelicopterPlatformData _helicopterPlatformData;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _helicopterPlatformData = persistentDataService.PersistentData.PlayerData.PlatformsData.HelicopterPlatformData;
        }

        public override void InstallBindings()
        {
            BindTransformVelocityAdapter();

            BindMinigunAmmo();
            BindAimer();
            BindBarrelSpiner();
            BindBarrelSpinAudio();
            BindStateMachine();
            BindWeapon();
            BindShootCameraShaker();
            BindShootParticle();
            BindShootAudioPlayer();
            BindShellSpawner();
            Container.Bind<ToggleableManager>().AsSingle();
        }

        private void BindTransformVelocityAdapter()
        {
            Container
                .BindInterfacesTo<AdaptedTransformForVelocity>()
                .AsSingle()
                .WithArguments(transform);
        }

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<MinigunStateFactory>().AsSingle();
            Container.BindInterfacesTo<MinigunStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<SpinUpState>().AsSingle();
            Container.Bind<AudioPlayer>().AsSingle().WithArguments(_fireAudioPreferences).WhenInjectedInto<ShootState>();
            Container.Bind<ShootState>().AsSingle().WithArguments(_firePreferences);
            Container.Bind<SpinDownState>().AsSingle();
            Container.Bind<IdleState>().AsSingle();
        }

        private void BindMinigunAmmo()
        {
            int ammoCapacity = _helicopterPlatformData.MinigunAmmoCapacity;

            ClampedIntegerBank ammo = new ClampedIntegerBank(ammoCapacity, ammoCapacity);
            Container.BindInstance(ammo).AsSingle();
        }

        private void BindAimer()
        {
            Container
                .BindInterfacesTo<WeaponAimer>()
                .AsSingle()
                .WithArguments(_aimPreferences);
        }

        private void BindBarrelSpiner()
        {
            Container
                .BindInterfacesAndSelfTo<BarrelSpiner>()
                .AsSingle()
                .WithArguments(_barrelRotatorPreferences);
        }

        private void BindBarrelSpinAudio()
        {
            Container.BindInterfacesTo<MinigunSpinAudio>().AsSingle().WithArguments(_spinAudioPreferences);
        }

        private void BindWeapon() => Container.BindInterfacesTo<Minigun>().AsSingle();

        private void BindShootCameraShaker() => Container.BindInterfacesTo<ShootCameraShaker>().AsSingle();

        private void BindShootParticle() =>
            Container.BindInterfacesTo<ShootParticle>().AsSingle().WithArguments(_shootParticlePreferences);

        private void BindShootAudioPlayer() =>
            Container.BindInterfacesTo<ShootAudioPlayer>().AsSingle().WithArguments(_fireAudioPreferences);

        private void BindShellSpawner() =>
            Container.BindInterfacesTo<ShellSpawner>().AsSingle().WithArguments(_shellSpawnerPreferences);
    }
}