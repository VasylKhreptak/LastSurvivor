using System;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class SpinUpState : IMinigunState, IPayloadedState<Action>
    {
        private readonly BarrelSpiner _barrelSpiner;

        public SpinUpState(BarrelSpiner barrelSpiner)
        {
            _barrelSpiner = barrelSpiner;
        }

        public void Enter(Action onComplete = null) => _barrelSpiner.SpinUp(() => onComplete?.Invoke());
    }
}