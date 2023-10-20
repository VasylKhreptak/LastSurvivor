using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory.Core;
using Zenject;

namespace Infrastructure.StateMachine.Main.States.Factory
{
    public abstract class StateFactory : IStateFactory, IFactory<Type, IBaseState>
    {
        protected readonly DiContainer _container;
        private readonly Dictionary<Type, Func<IBaseState>> _statesMap;

        protected StateFactory(DiContainer container)
        {
            _container = container;
            _statesMap = BuildStatesRegister();
        }

        protected abstract Dictionary<Type, Func<IBaseState>> BuildStatesRegister();

        public T GetState<T>() where T : class, IBaseState => Create(typeof(T)) as T;

        public IBaseState Create(Type type)
        {
            if (_statesMap.TryGetValue(type, out Func<IBaseState> state))
                return state();

            throw new Exception($"State for {type.Name} can't be created");
        }
    }
}