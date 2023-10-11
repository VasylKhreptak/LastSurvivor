using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using Zenject;

namespace Infrastructure.StateMachine.Game.Factory
{
    public class GameStateFactory : StateFactory
    {
        private readonly Dictionary<Type, IBaseState> _cachedStatesMap;

        public GameStateFactory(DiContainer container) : base(container)
        {
            _cachedStatesMap = new Dictionary<Type, IBaseState>();
        }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister()
        {
            return new Dictionary<Type, Func<IBaseState>>()
            {
                [typeof(BootstrapState)] = Get<BootstrapState>,
                [typeof(SetupApplicationState)] = Get<SetupApplicationState>,
                [typeof(LoadDataState)] = Get<LoadDataState>,
                [typeof(BootstrapAnalyticsState)] = Get<BootstrapAnalyticsState>,
                [typeof(LoadLevelState)] = Get<LoadLevelState>,
                [typeof(GameLoopState)] = Get<GameLoopState>,
            };
        }

        private IBaseState Get<TState>() where TState : IBaseState
        {
            if (_cachedStatesMap.TryGetValue(typeof(TState), out IBaseState state))
            {
                return state;
            }

            state = _container.Resolve<TState>();
            _cachedStatesMap.Add(typeof(TState), state);
            return state;
        }
    }
}
