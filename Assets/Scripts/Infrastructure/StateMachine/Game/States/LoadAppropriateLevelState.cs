using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadAppropriateLevelState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;

        public LoadAppropriateLevelState(IStateMachine<IGameState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter() => Debug.Log("LoadAppropriteLevelState");
    }
}