using Audio.Players;
using Data.Persistent.Platforms;
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
        [SerializeField] private FireState.Preferences _firePreferences;
        [SerializeField] private AudioPlayer.Preferences _fireAudioPreferences;
        [SerializeField] private MinigunSpinAudio.Preferences _spinAudioPreferences;

        private HelicopterPlatformData _helicopterPlatformData;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _helicopterPlatformData = persistentDataService.PersistentData.PlayerData.PlatformsData.HelicopterPlatformData;
        }

        public override void InstallBindings()
        {
            BindMinigunAmmo();
            BindAimer();
            BindBarrelSpiner();
            BindBarrelSpinAudio();
            BindStateMachine();
            Container.Bind<ToggleableManager>().AsSingle();
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
            Container.Bind<AudioPlayer>().AsSingle().WithArguments(_fireAudioPreferences).WhenInjectedInto<FireState>();
            Container.Bind<FireState>().AsSingle().WithArguments(_firePreferences);
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
    }
}