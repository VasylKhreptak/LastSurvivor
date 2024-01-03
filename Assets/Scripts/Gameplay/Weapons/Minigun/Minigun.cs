using Gameplay.Weapons.Core;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Plugins.Banks;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Minigun
{
    public class Minigun : MonoBehaviour, IWeapon
    {
        private IStateMachine<IMinigunState> _stateMachine;
        private ClampedIntegerBank _ammo;

        [Inject]
        private void Constructor(IStateMachine<IMinigunState> stateMachine, ClampedIntegerBank ammo)
        {
            _stateMachine = stateMachine;
            _ammo = ammo;
        }

        public ClampedIntegerBank Ammo => _ammo;

        public void StartShooting() => _stateMachine.Enter<SpinUpState>();

        public void StopShooting() => _stateMachine.Enter<SpinDownState>();
    }
}