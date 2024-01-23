using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Utilities.PhysicsUtilities
{
    public class TriggerZone<T> : IInitializable, IDisposable
    {
        private readonly Collider _trigger;
        private readonly GameObject _gameObject;

        public TriggerZone(Collider trigger)
        {
            _trigger = trigger;
        }

        private readonly IReactiveCollection<TriggerInfo<T>> _triggers = new ReactiveCollection<TriggerInfo<T>>();

        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        public IReadOnlyReactiveCollection<TriggerInfo<T>> Triggers => _triggers;

        public void Initialize()
        {
            StartObservingOnDisable();
            StartObservingTrigger();
        }

        public void Dispose() => StopObserving();

        private void StopObserving() => _subscriptions.Clear();

        private void StartObservingOnDisable()
        {
            _gameObject.OnDisableAsObservable().Subscribe(_ => OnDisabled()).AddTo(_subscriptions);
        }

        private void OnDisabled() => _triggers.Clear();

        private void StartObservingTrigger()
        {
            _trigger.OnTriggerEnterAsObservable().Subscribe(OnTriggerEnter).AddTo(_subscriptions);
            _trigger.OnTriggerExitAsObservable().Subscribe(OnTriggerExit).AddTo(_subscriptions);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out T value))
                _triggers.Add(new TriggerInfo<T>(other.transform, value));
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerInfo<T> triggerInfo = _triggers.FirstOrDefault(info => info.Transform == other.transform);

            if (triggerInfo != null)
                _triggers.Remove(triggerInfo);
        }
    }

    public class TriggerInfo<T>
    {
        public readonly Transform Transform;
        public readonly T Value;

        public TriggerInfo(Transform transform, T value)
        {
            Transform = transform;
            Value = value;
        }
    }
}