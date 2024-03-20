using System;
using Gameplay.Weapons.Core;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Minigun
{
    public class Minigun : MonoBehaviour, IWeapon
    {
        private IStateMachine<IMinigunState> _stateMachine;
        private ReloadState _reloadState;

        [Inject]
        private void Constructor(Transform transform, IStateMachine<IMinigunState> stateMachine, ClampedIntegerBank ammo,
            ReloadState reloadState)
        {
            Transform = transform;
            _stateMachine = stateMachine;
            Ammo = ammo;
            _reloadState = reloadState;
        }

        private bool _isReloading;
        private bool _isShooting;

        public Transform Transform { get; private set; }

        public ClampedIntegerBank Ammo { get; private set; }

        public IReadOnlyReactiveProperty<float> ReloadProgress => _reloadState.ReloadProgress;

        public IReadOnlyReactiveProperty<bool> IsReloading => _reloadState.IsReloading;

        public void StartShooting()
        {
            _isShooting = true;

            if (_stateMachine.ActiveStateType == typeof(ReloadState))
                return;

            _stateMachine.Enter<SpinUpState, Action>(() => { _stateMachine.Enter<ShootState, Action>(OnReloaded); });
        }

        public void StopShooting()
        {
            _isShooting = false;

            if (_stateMachine.ActiveStateType == typeof(ReloadState))
                return;

            _stateMachine.Enter<SpinDownState>();
        }

        private void OnReloaded()
        {
            if (_isShooting)
                StartShooting();
        }
    }
}