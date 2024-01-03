using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class SpinDownState : IMinigunState
    {
        private readonly IStateMachine<IMinigunState> _stateMachine;
        private readonly BarrelRotator _barrelRotator;

        public SpinDownState(IStateMachine<IMinigunState> stateMachine, BarrelRotator barrelRotator)
        {
            _stateMachine = stateMachine;
            _barrelRotator = barrelRotator;
        }

        public void Enter() => _barrelRotator.SpinDown(() => _stateMachine.Enter<IdleState>());
    }
}