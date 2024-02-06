using Gameplay.Entities.Collector.StateMachine.States;
using Gameplay.Entities.Collector.StateMachine.States.Core;
using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using Infrastructure.StateMachine.Main.Core;

namespace Gameplay.Entities.Collector
{
    public class CollectorDeathHandler : DeathHandler
    {
        private readonly IStateMachine<ICollectorState> _stateMachine;

        public CollectorDeathHandler(IStateMachine<ICollectorState> stateMachine, IHealth health) : base(health)
        {
            _stateMachine = stateMachine;
        }

        protected override void OnDied() => _stateMachine.Enter<DeathState>();
    }
}