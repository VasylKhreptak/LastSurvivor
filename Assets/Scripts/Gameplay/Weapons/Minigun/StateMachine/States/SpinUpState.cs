using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class SpinUpState : IMinigunState
    {
        private readonly IStateMachine<IMinigunState> _stateMachine;
        private readonly BarrelSpiner _barrelSpiner;

        public SpinUpState(IStateMachine<IMinigunState> stateMachine, BarrelSpiner barrelSpiner)
        {
            _stateMachine = stateMachine;
            _barrelSpiner = barrelSpiner;
        }

        public void Enter() => _barrelSpiner.SpinUp(() => _stateMachine.Enter<FireState>());
    }
}