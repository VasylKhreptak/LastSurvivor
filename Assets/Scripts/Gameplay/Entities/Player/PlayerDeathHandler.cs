using Gameplay.Entities.Health;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;

namespace Gameplay.Entities.Player
{
    public class PlayerDeathHandler : DeathHandler
    {
        private readonly IStateMachine<IPlayerState> _stateMachine;

        public PlayerDeathHandler(IStateMachine<IPlayerState> stateMachine, IHealth health) : base(health)
        {
            _stateMachine = stateMachine;
        }

        protected override void OnDied() => _stateMachine.Enter<DeathState>();
    }
}