using System;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Info.Core;

namespace Infrastructure.StateMachine.Main.States.Info
{
    public class StateInfo<TState, TBaseState> : IStateInfo where TState : class, IState, TBaseState
    {
        private readonly StateMachine<TBaseState> _stateMachine;
        private readonly TState _state;
        private readonly IUpdatable _updatable;
        private readonly IExitable _exitable;

        public StateInfo(StateMachine<TBaseState> stateMachine, TState state)
        {
            _stateMachine = stateMachine;
            _state = state;
            _updatable = state as IUpdatable;
            _exitable = state as IExitable;
            StateType = typeof(TState);
        }

        public Type StateType { get; }

        public virtual void Enter() => _stateMachine.Enter<TState>();

        public void Update() => _updatable?.Update();

        public void Exit() => _exitable?.Exit();
    }
}
