using System;
using Gameplay.Weapons.Core;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Holders.Core;
using Infrastructure.StateMachine.Main.Core;
using Plugins.Banks;
using UniRx;
using UnityEngine;

namespace Gameplay.Weapons.Minigun
{
    public class Minigun : IWeapon
    {
        private readonly IStateMachine<IMinigunState> _stateMachine;
        private readonly ReloadState _reloadState;

        public Minigun(Transform transform, IStateMachine<IMinigunState> stateMachine, ClampedIntegerBank ammo,
            ReloadState reloadState)
        {
            Transform = transform;
            _stateMachine = stateMachine;
            Ammo = ammo;
            _reloadState = reloadState;
        }

        private readonly InstanceHolder<Action> _spinUpPayload = new InstanceHolder<Action>();

        private readonly InstanceHolder<Action> _reloadStatePayload = new InstanceHolder<Action>();

        private bool _isReloading;

        public Transform Transform { get; }

        public ClampedIntegerBank Ammo { get; }

        public IReadOnlyReactiveProperty<float> ReloadProgress => _reloadState.ReloadProgress;

        public void StartShooting()
        {
            _reloadStatePayload.Instance = StartShooting;

            if (_stateMachine.ActiveStateType == typeof(ReloadState))
                return;

            _stateMachine.Enter<SpinUpState, Action>(() =>
            {
                _stateMachine.Enter<ShootState, InstanceHolder<Action>>(_reloadStatePayload);
            });
        }

        public void StopShooting()
        {
            if (_stateMachine.ActiveStateType == typeof(ReloadState))
            {
                _reloadStatePayload.Instance = null;
                return;
            }

            _stateMachine.Enter<SpinDownState>();

            _reloadStatePayload.Instance = null;
        }
    }
}