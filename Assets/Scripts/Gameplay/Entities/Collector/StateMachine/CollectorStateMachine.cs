using Gameplay.Entities.Collector.StateMachine.States.Core;
using Infrastructure.StateMachine.Main;

namespace Gameplay.Entities.Collector.StateMachine
{
    public class CollectorStateMachine : StateMachine<ICollectorState>
    {
        protected CollectorStateMachine(CollectorStateFactory stateFactory) : base(stateFactory) { }
    }
}