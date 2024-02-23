using System;
using Gameplay.Weapons.Core;
using Gameplay.Weapons.Minigun.StateMachine.States;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Holders.Core;
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

        private readonly InstanceHolder<Action> _reloadStatePayload = new InstanceHolder<Action>();

        private bool _isReloading;

        public Transform Transform { get; private set; }

        public ClampedIntegerBank Ammo { get; private set; }

        public IReadOnlyReactiveProperty<float> ReloadProgress => _reloadState.ReloadProgress;

        public IReadOnlyReactiveProperty<bool> IsReloading => _reloadState.IsReloading;

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