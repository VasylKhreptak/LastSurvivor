using System;
using DG.Tweening;
using Tags.Gameplay;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;
using Zenject.Infrastructure.Toggleable.Core;

namespace Gameplay.Entities.Loot
{
    public class LootAttractor : IEnableable, IDisableable
    {
        private readonly Transform _transform;
        private readonly ClosestTriggerObserver<LootCollector> _closestCollectorObserver;
        private readonly CollectHandler _collectHandler;
        private readonly Preferences _preferences;

        public LootAttractor(Transform transform, ClosestTriggerObserver<LootCollector> closestCollectorObserver,
            CollectHandler collectHandler, Preferences preferences)
        {
            _transform = transform;
            _closestCollectorObserver = closestCollectorObserver;
            _collectHandler = collectHandler;
            _preferences = preferences;
        }

        private IDisposable _closestCollectorObserverSubscription;
        private IDisposable _delaySubscription;
        private IDisposable _attractDelaySubscription;

        private Tween _tween;

        public void Enable()
        {
            _delaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(_preferences.Delay))
                .Subscribe(_ => StartObserving());
        }

        public void Disable()
        {
            _delaySubscription?.Dispose();
            _attractDelaySubscription?.Dispose();
            StopObserving();
            Stop();
        }

        private void StartObserving()
        {
            StopObserving();
            _closestCollectorObserverSubscription = _closestCollectorObserver.Trigger
                .Select(x => x?.Value)
                .Subscribe(OnClosestCollectorChanged);
        }

        private void StopObserving() => _closestCollectorObserverSubscription?.Dispose();

        private void OnClosestCollectorChanged(LootCollector collector)
        {
            _attractDelaySubscription?.Dispose();

            if (collector == null)
                return;

            _attractDelaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(UnityEngine.Random.Range(_preferences.MinAttractDelay, _preferences.MaxAttractDelay)))
                .Subscribe(_ => Attract(collector.Target));
        }

        private void Attract(Transform transform)
        {
            Stop();

            float progress = 0f;
            _tween = DOTween
                .To(() => progress, x => progress = x, 1f, _preferences.AttractDuration)
                .SetEase(_preferences.Curve)
                .OnUpdate(() =>
                {
                    _transform.position = Vector3.Slerp(_transform.position, transform.position, progress);
                    _transform.localScale = Vector3.Lerp(_transform.localScale, _preferences.TargetScale, progress);
                })
                .OnComplete(() => _collectHandler.HandleCollect())
                .Play();
        }

        private void Stop() => _tween.Kill();

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _delay = 0.5f;
            [SerializeField] private float _minAttractDelay = 0.1f;
            [SerializeField] private float _maxAttractDelay = 0.5f;
            [SerializeField] private float _attractDuration = 0.5f;
            [SerializeField] private Vector3 _targetScale = Vector3.one * 0.2f;
            [SerializeField] private AnimationCurve _curve;

            public float Delay => _delay;
            public float MinAttractDelay => _minAttractDelay;
            public float MaxAttractDelay => _maxAttractDelay;
            public float AttractDuration => _attractDuration;
            public Vector3 TargetScale => _targetScale;
            public AnimationCurve Curve => _curve;
        }
    }
}