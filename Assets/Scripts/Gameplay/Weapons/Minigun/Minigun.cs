using Gameplay.Weapons.Core;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Plugins.Banks;

namespace Gameplay.Weapons.Minigun
{
    public class Minigun : IWeapon
    {
        private readonly IStateMachine<IMinigunState> _stateMachine;
        private readonly ShootState _shootState;

        public Minigun(IStateMachine<IMinigunState> stateMachine, ShootState shootState, ClampedIntegerBank ammo)
        {
            _stateMachine = stateMachine;
            _shootState = shootState;
            Ammo = ammo;
        }

        public ClampedIntegerBank Ammo { get; }

        public void StartShooting() => _stateMachine.Enter<SpinUpState>();

        public void StopShooting() => _stateMachine.Enter<SpinDownState>();
    }
}