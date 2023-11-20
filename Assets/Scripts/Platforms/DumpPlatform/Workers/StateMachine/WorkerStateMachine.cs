using Infrastructure.StateMachine.Main;
using Platforms.DumpPlatform.Workers.StateMachine.States.Core;

namespace Platforms.DumpPlatform.Workers.StateMachine
{
    public class WorkerStateMachine : StateMachine<IWorkerState>
    {
        protected WorkerStateMachine(WorkerStateFactory stateFactory) : base(stateFactory) { }
    }
}