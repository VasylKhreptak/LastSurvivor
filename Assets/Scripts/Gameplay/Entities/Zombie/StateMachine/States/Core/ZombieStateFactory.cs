using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using Zenject;

namespace Gameplay.Entities.Zombie.StateMachine.States.Core
{
    public class ZombieStateFactory : StateFactory
    {
        public ZombieStateFactory(DiContainer container) : base(container) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                [typeof(IdleState)] = _container.Resolve<IdleState>,
                [typeof(FollowTransformState)] = _container.Resolve<FollowTransformState>,
                [typeof(DeathState)] = _container.Resolve<DeathState>
            };
    }
}