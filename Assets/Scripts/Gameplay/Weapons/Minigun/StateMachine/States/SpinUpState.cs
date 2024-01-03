using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class SpinUpState : IMinigunState
    {
        private readonly IStateMachine<IMinigunState> _stateMachine;
        private readonly BarrelRotator _barrelRotator;

        public SpinUpState(IStateMachine<IMinigunState> stateMachine, BarrelRotator barrelRotator)
        {
            _stateMachine = stateMachine;
            _barrelRotator = barrelRotator;
        }

        public void Enter() => _barrelRotator.SpinUp(() => _stateMachine.Enter<FireState>());
    }
}