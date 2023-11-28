using Infrastructure.StateMachine.Main.States.Core;
using Platforms.DumpPlatform.Workers.StateMachine.States.Core;

namespace Platforms.DumpPlatform.Workers.StateMachine.States
{
    public class IdleState : IState, IWorkerState
    {
        public void Enter() { }
    }
}