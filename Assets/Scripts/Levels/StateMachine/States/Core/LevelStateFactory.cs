using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using Zenject;

namespace Levels.StateMachine.States.Core
{
    public class LevelStateFactory : StateFactory
    {
        public LevelStateFactory(DiContainer container) : base(container) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                [typeof(StartLevelState)] = _container.Resolve<StartLevelState>,
                [typeof(FailLevelState)] = _container.Resolve<FailLevelState>,
                [typeof(FinishLevelState)] = _container.Resolve<FinishLevelState>
            };
    }
}