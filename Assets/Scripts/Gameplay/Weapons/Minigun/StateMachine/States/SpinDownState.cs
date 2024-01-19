using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class SpinDownState : IMinigunState
    {
        private readonly IStateMachine<IMinigunState> _stateMachine;
        private readonly BarrelSpiner _barrelSpiner;

        public SpinDownState(IStateMachine<IMinigunState> stateMachine, BarrelSpiner barrelSpiner)
        {
            _stateMachine = stateMachine;
            _barrelSpiner = barrelSpiner;
        }

        public void Enter() => _barrelSpiner.SpinDown(() => _stateMachine.Enter<IdleState>());
    }
}