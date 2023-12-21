using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using Zenject;

namespace Gameplay.Weapons.Minigun.StateMachine.States.Core
{
    public class MinigunStateFactory : StateFactory
    {
        public MinigunStateFactory(DiContainer container) : base(container) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                [typeof(SpinUpState)] = _container.Resolve<SpinUpState>,
                [typeof(LoopState)] = _container.Resolve<LoopState>,
                [typeof(SpinDownState)] = _container.Resolve<SpinDownState>,
                [typeof(IdleState)] = _container.Resolve<IdleState>
            };
    }
}