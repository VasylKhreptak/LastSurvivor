using System;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Info.Core;

namespace Infrastructure.StateMachine.Main.States.Info
{
    public class PayloadedStateInfo<TState, TBaseState, TPayload> : IStateInfo where TState : class, IPayloadedState<TPayload>, TBaseState
    {
        private readonly StateMachine<TBaseState> _stateMachine;
        private readonly TPayload _payload;
        private readonly IUpdatable _updatable;
        private readonly IExitable _exitable;

        public PayloadedStateInfo(StateMachine<TBaseState> stateMachine, TState state, TPayload payload)
        {
            _stateMachine = stateMachine;
            _payload = payload;

            _updatable = state as IUpdatable;
            _exitable = state as IExitable;

            StateType = typeof(TState);
        }

        public Type StateType { get; }

        public void Enter() => _stateMachine.Enter<TState, TPayload>(_payload);

        public void Update() => _updatable?.Update();

        public void Exit() => _exitable?.Exit();
    }
}
