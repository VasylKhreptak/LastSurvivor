using System;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.Transition.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadSceneWithTransitionAsyncState : IGameState, IPayloadedState<LoadSceneWithTransitionAsyncState.Payload>
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ITransitionScreen _transitionScreen;

        public LoadSceneWithTransitionAsyncState(IStateMachine<IGameState> stateMachine, ITransitionScreen transitionScreen)
        {
            _stateMachine = stateMachine;
            _transitionScreen = transitionScreen;
        }

        public void Enter(Payload payload)
        {
            _transitionScreen.Show(() =>
            {
                payload.OnTransitionShown?.Invoke();
                payload.LoadScenePayload.OnComplete += () => _transitionScreen.Hide();

                _stateMachine.Enter<LoadSceneAsyncState, LoadSceneAsyncState.Payload>(payload.LoadScenePayload);
            });
        }

        [Serializable]
        public class Payload
        {
            public LoadSceneAsyncState.Payload LoadScenePayload;
            public Action OnTransitionShown;
        }
    }
}