using Gameplay.Weapons.Core;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Plugins.Banks;
using UnityEngine;

namespace Gameplay.Weapons.Minigun
{
    public class Minigun : IWeapon
    {
        private readonly Transform _transform;
        private readonly IStateMachine<IMinigunState> _stateMachine;
        private readonly ClampedIntegerBank _ammo;

        public Minigun(Transform transform, IStateMachine<IMinigunState> stateMachine, ClampedIntegerBank ammo)
        {
            _transform = transform;
            _stateMachine = stateMachine;
            _ammo = ammo;
        }

        public Transform Transform => _transform;

        public ClampedIntegerBank Ammo => _ammo;

        public void StartShooting() => _stateMachine.Enter<SpinUpState>();

        public void StopShooting() => _stateMachine.Enter<SpinDownState>();
    }
}