using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Utilities.PhysicsUtilities.Trigger
{
    public class ClosestTriggerObserver<T> : IInitializable, IDisposable
    {
        private readonly TriggerZone<T> _triggerZone;
        private readonly Preferences _preferences;

        public ClosestTriggerObserver(TriggerZone<T> triggerZone, Preferences preferences)
        {
            _triggerZone = triggerZone;
            _preferences = preferences;
        }

        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        private readonly ReactiveProperty<TriggerInfo<T>> _trigger = new ReactiveProperty<TriggerInfo<T>>();

        public IReadOnlyReactiveProperty<TriggerInfo<T>> Trigger => _trigger;

        public void Initialize() => StartObserving();

        public void Dispose()
        {
            StopObserving();
            _trigger.Value = null;
        }

        private void StartObserving()
        {
            Observable
                .Interval(TimeSpan.FromSeconds(_preferences.UpdateInterval))
                .DoOnSubscribe(UpdateClosestTrigger)
                .Subscribe(_ => UpdateClosestTrigger())
                .AddTo(_subscriptions);

            _triggerZone.Triggers
                .ObserveCountChanged()
                .Subscribe(_ => UpdateClosestTrigger())
                .AddTo(_subscriptions);
        }

        private void StopObserving() => _subscriptions.Clear();

        private void UpdateClosestTrigger()
        {
            TriggerInfo<T> closestTrigger = null;
            float closestDistance = float.MaxValue;

            foreach (TriggerInfo<T> trigger in _triggerZone.Triggers)
            {
                float distance = Vector3.Distance(trigger.Transform.position, _triggerZone.Center);

                if (distance < closestDistance)
                {
                    closestTrigger = trigger;
                    closestDistance = distance;
                }
            }

            _trigger.Value = closestTrigger;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _updateInterval = 0.2f;

            public float UpdateInterval => _updateInterval;
        }
    }
}