using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Soldier.StateMachine.States;
using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;

namespace Gameplay.Entities.Soldier
{
    public class SoldierDeathHandler : DeathHandler
    {
        private readonly IStateMachine<ISoldierState> _stateMachine;

        public SoldierDeathHandler(IStateMachine<ISoldierState> stateMachine, IHealth health) : base(health)
        {
            _stateMachine = stateMachine;
        }

        protected override void OnDied() => _stateMachine.Enter<DeathState>();
    }
}