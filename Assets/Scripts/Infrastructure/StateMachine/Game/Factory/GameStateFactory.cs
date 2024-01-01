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
        public GameStateFactory(DiContainer container) : base(container) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                [typeof(BootstrapState)] = _container.Resolve<BootstrapState>,
                [typeof(SetupApplicationState)] = _container.Resolve<SetupApplicationState>,
                [typeof(LoadDataState)] = _container.Resolve<LoadDataState>,
                [typeof(BootstrapAnalyticsState)] = _container.Resolve<BootstrapAnalyticsState>,
                [typeof(FinalyzeBootstrapState)] = _container.Resolve<FinalyzeBootstrapState>,
                [typeof(LoadSceneAsyncState)] = _container.Resolve<LoadSceneAsyncState>,
                [typeof(GameLoopState)] = _container.Resolve<GameLoopState>,
                [typeof(PlayState)] = _container.Resolve<PlayState>,
                [typeof(LoadAppropriateLevelState)] = _container.Resolve<LoadAppropriateLevelState>
            };
    }
}