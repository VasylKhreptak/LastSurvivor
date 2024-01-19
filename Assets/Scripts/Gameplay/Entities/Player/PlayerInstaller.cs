using Gameplay.Entities.Player.StateMachine;
using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStateMachine();
        }

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<PlayerStateFactory>().AsSingle();
            Container.BindInterfacesTo<PlayerStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<IdleState>().AsSingle();
            Container.Bind<MoveState>().AsSingle();
        }
    }
}