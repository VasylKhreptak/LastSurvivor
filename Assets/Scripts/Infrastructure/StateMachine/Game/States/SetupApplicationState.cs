using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace Infrastructure.StateMachine.Game.States
{
    public class SetupApplicationState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;

        public SetupApplicationState(IStateMachine<IGameState> gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            DisableSleepTimeout();
            _gameStateMachine.Enter<LoadDataState>();
        }

        private void DisableSleepTimeout() => Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}