using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using JetBrains.Annotations;

namespace DebuggerOptions
{
    public class SROptionsContainer
    {
        private readonly IStateMachine<IGameState> _stateMachine;

        public SROptionsContainer(IStateMachine<IGameState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        [UsedImplicitly]
        public void EnterBootstrapState() => _stateMachine.Enter<BootstrapState>();
    }
}