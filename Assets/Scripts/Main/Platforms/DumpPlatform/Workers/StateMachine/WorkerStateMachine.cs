using Infrastructure.StateMachine.Main;
using Main.Platforms.DumpPlatform.Workers.StateMachine.States.Core;

namespace Main.Platforms.DumpPlatform.Workers.StateMachine
{
    public class WorkerStateMachine : StateMachine<IWorkerState>
    {
        protected WorkerStateMachine(WorkerStateFactory stateFactory) : base(stateFactory) { }
    }
}