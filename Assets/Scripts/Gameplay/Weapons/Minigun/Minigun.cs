using System;
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
        private readonly ClampedIntegerBank _ammo;

        public Minigun(IStateMachine<IMinigunState> stateMachine, ShootState shootState, ClampedIntegerBank ammo)
        {
            _stateMachine = stateMachine;
            _shootState = shootState;
            _ammo = ammo;
        }

        public event Action<ShootData> OnShoot { add => _shootState.OnShoot += value; remove => _shootState.OnShoot -= value; }

        public ClampedIntegerBank Ammo => _ammo;

        public void StartShooting() => _stateMachine.Enter<SpinUpState>();

        public void StopShooting() => _stateMachine.Enter<SpinDownState>();
    }
}