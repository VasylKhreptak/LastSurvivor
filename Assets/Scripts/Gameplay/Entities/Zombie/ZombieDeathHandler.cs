using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;

namespace Gameplay.Entities.Zombie
{
    public class ZombieDeathHandler : DeathHandler
    {
        private readonly IStateMachine<IZombieState> _stateMachine;

        public ZombieDeathHandler(IStateMachine<IZombieState> stateMachine, IHealth health) : base(health)
        {
            _stateMachine = stateMachine;
        }

        protected override void OnDied() => _stateMachine.Enter<DeathState>();
    }
}