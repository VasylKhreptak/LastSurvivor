using System;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Plugins.Banks;
using Plugins.Timer;
using UniRx;
using UnityEngine;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class ReloadState : IMinigunState, IPayloadedState<Action>, IExitable
    {
        private readonly IStateMachine<IMinigunState> _stateMachine;
        private readonly ClampedIntegerBank _ammo;
        private readonly BarrelSpiner _barrelSpiner;
        private readonly Preferences _preferences;

        public ReloadState(IStateMachine<IMinigunState> stateMachine, ClampedIntegerBank ammo, BarrelSpiner barrelSpiner,
            Preferences preferences)
        {
            _stateMachine = stateMachine;
            _ammo = ammo;
            _barrelSpiner = barrelSpiner;
            _preferences = preferences;
        }

        private readonly ITimer _reloadTimer = new Timer();

        private IDisposable _completeSubscription;

        public IReadOnlyReactiveProperty<float> ReloadProgress => _reloadTimer.Progress;

        private readonly BoolReactiveProperty _isReloading = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsReloading => _isReloading;

        public void Enter(Action onComplete = null)
        {
            _barrelSpiner.SpinDown();
            _reloadTimer.Start(_preferences.Duration);
            _isReloading.Value = true;
            _completeSubscription = _reloadTimer.OnCompleted.Subscribe(_ =>
            {
                _stateMachine.Enter<IdleState>();
                _ammo.Fill();
                onComplete?.Invoke();
            });
        }

        public void Exit()
        {
            _reloadTimer.Reset();
            _completeSubscription?.Dispose();
            _isReloading.Value = false;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _duration = 2f;

            public float Duration => _duration;
        }
    }
}