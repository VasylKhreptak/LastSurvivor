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
        private readonly IStateMachine<IMinigunState> _stateMachine;

        public Minigun(Transform transform, IStateMachine<IMinigunState> stateMachine, ClampedIntegerBank ammo)
        {
            Transform = transform;
            _stateMachine = stateMachine;
            Ammo = ammo;
        }

        public Transform Transform { get; }

        public ClampedIntegerBank Ammo { get; }

        public void StartShooting() => _stateMachine.Enter<SpinUpState>();

        public void StopShooting() => _stateMachine.Enter<SpinDownState>();
    }
}