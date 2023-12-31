using Data.Persistent.Platforms;
using Gameplay.Weapons.Minigun.StateMachine;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Plugins.Banks;
using Zenject;

namespace Gameplay.Weapons.Minigun
{
    public class MinigunInstaller : MonoInstaller
    {
        private HelicopterPlatformData _platformData;

        [Inject]
        private void Constructor(HelicopterPlatformData platformData)
        {
            _platformData = platformData;
        }

        public override void InstallBindings()
        {
            BindMagazine();
            BindStateMachine();
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
            Container.Bind<LoopState>().AsSingle();
            Container.Bind<SpinDownState>().AsSingle();
            Container.Bind<IdleState>().AsSingle();
        }

        private void BindMagazine()
        {
            ClampedIntegerBank clampedIntegerBank =
                new ClampedIntegerBank(_platformData.MinigunAmmoCapacity, _platformData.MinigunAmmoCapacity);
        }
    }
}