using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using Zenject;

namespace Main.Platforms.DumpPlatform.Workers.StateMachine.States.Core
{
    public class WorkerStateFactory : StateFactory
    {
        public WorkerStateFactory(DiContainer container) : base(container) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                [typeof(WorkState)] = _container.Resolve<WorkState>, [typeof(IdleState)] = _container.Resolve<IdleState>
            };
    }
}