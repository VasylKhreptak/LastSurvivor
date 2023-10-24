using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable.Core;

namespace Lifetime
{
    [DisallowMultipleComponent]
    public class ObjectLifetimeProvider : MonoBehaviour
    {
        private readonly HashSet<IInitializable> _initializables = new HashSet<IInitializable>();
        private readonly HashSet<IEnableable> _enableables = new HashSet<IEnableable>();
        private readonly HashSet<ITickable> _tickables = new HashSet<ITickable>();
        private readonly HashSet<IFixedTickable> _fixedTickables = new HashSet<IFixedTickable>();
        private readonly HashSet<ILateTickable> _lateTickables = new HashSet<ILateTickable>();
        private readonly HashSet<IDisableable> _disableables = new HashSet<IDisableable>();
        private readonly HashSet<IDisposable> _disposables = new HashSet<IDisposable>();

        private bool _isInitialized;

        #region MonoBehaviour

        private void Awake()
        {
            _initializables.ForEach(initializable => initializable.Initialize());
            _isInitialized = true;
        }

        private void OnEnable() => _enableables.ForEach(enableable => enableable.Enable());
        private void Update() => _tickables.ForEach(tickable => tickable.Tick());
        private void FixedUpdate() => _fixedTickables.ForEach(fixedTickable => fixedTickable.FixedTick());
        private void LateUpdate() => _lateTickables.ForEach(lateTickable => lateTickable.LateTick());
        private void OnDisable() => _disableables.ForEach(disableable => disableable.Disable());
        private void OnDestroy() => _disposables.ForEach(disposable => disposable.Dispose());

        #endregion

        public void Add(IInitializable initializable)
        {
            if (_initializables.Add(initializable))
            {
                if (_isInitialized)
                    initializable.Initialize();
            }
        }

        public void Add(IEnableable enableable)
        {
            if (_enableables.Add(enableable))
            {
                if (enabled && _isInitialized)
                    enableable.Enable();
            }
        }

        public void Add(ITickable tickable) => _tickables.Add(tickable);

        public void Add(IFixedTickable fixedTickable) => _fixedTickables.Add(fixedTickable);

        public void Add(ILateTickable lateTickable) => _lateTickables.Add(lateTickable);

        public void Add(IDisableable disableable)
        {
            if (_disableables.Add(disableable))
            {
                if (_isInitialized && enabled == false)
                    disableable.Disable();
            }
        }

        public void Add(IDisposable disposable) => _disposables.Add(disposable);
    }
}