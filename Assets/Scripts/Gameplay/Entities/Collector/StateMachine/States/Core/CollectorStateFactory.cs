using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using Zenject;

namespace Gameplay.Entities.Collector.StateMachine.States.Core
{
    public class CollectorStateFactory : StateFactory
    {
        public CollectorStateFactory(DiContainer container) : base(container) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                [typeof(IdleState)] = _container.Resolve<IdleState>,
                [typeof(MapNavigationState)] = _container.Resolve<MapNavigationState>,
                [typeof(DeathState)] = _container.Resolve<DeathState>
            };
    }
}