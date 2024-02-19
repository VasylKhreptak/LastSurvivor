using System;
using System.Collections.Generic;
using Balance;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Zombie;
using Gameplay.Entities.Zombie.StateMachine.States;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;
using Visitor;
using Zenject;
using Zenject.Infrastructure.Toggleable.Core;
using Object = UnityEngine.Object;

namespace Gameplay.Levels.ZombieSpawner
{
    public class ZombieSpawner : IEnableable, IDisableable, IDisposable
    {
        private readonly Transform _transform;
        private readonly GameObject _gameObject;
        private readonly DiContainer _container;
        private readonly GamePrefabs _prefabs;
        private readonly ClosestTriggerObserver<IVisitable<ZombieDamage>> _closestTargetObserver;
        private readonly List<ZombieSpawner> _zombieSpawners;
        private readonly float _spawnInterval;
        private readonly int _spawnCount;

        public ZombieSpawner(Transform transform, GameObject gameObject, DiContainer container, IStaticDataService staticDataService,
            ClosestTriggerObserver<IVisitable<ZombieDamage>> closestTargetObserver, List<ZombieSpawner> zombieSpawners,
            IPersistentDataService persistentDataService, Preferences preferences)
        {
            _transform = transform;
            _gameObject = gameObject;
            _container = container;
            _prefabs = staticDataService.Prefabs;
            _closestTargetObserver = closestTargetObserver;
            _zombieSpawners = zombieSpawners;

            _spawnInterval = preferences.Interval.Get(persistentDataService.Data.PlayerData.CompletedLevels);
            _spawnCount = (int)preferences.Count.Get(persistentDataService.Data.PlayerData.CompletedLevels);
        }

        private int _spawnedCount;

        private IDisposable _targetAviabilitySubscription;
        private IDisposable _spawnSubscription;

        public void Dispose()
        {
            Disable();
            _zombieSpawners.Remove(this);
        }

        public void Enable()
        {
            Disable();
            StartObservingTargetAviability();
        }

        public void Disable()
        {
            StopObservingTargetAviability();
            StopSpawning();
        }

        private void StartObservingTargetAviability()
        {
            _targetAviabilitySubscription = _closestTargetObserver.Trigger
                .Select(x => x != null)
                .Subscribe(OnTargetAviabilityChanged);
        }

        private void StopObservingTargetAviability() => _targetAviabilitySubscription?.Dispose();

        private void OnTargetAviabilityChanged(bool isAvailable)
        {
            if (isAvailable)
                StartSpawning();
            else
                StopSpawning();
        }

        private void StartSpawning()
        {
            StopSpawning();

            _spawnSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_spawnInterval))
                .DoOnSubscribe(Spawn)
                .Subscribe(_ => Spawn());
        }

        private void StopSpawning() => _spawnSubscription?.Dispose();

        private void Spawn()
        {
            GameObject spawnedZombieObject =
                _container.InstantiatePrefab(_prefabs[Prefab.Zombie], _transform.position, GetRotation(), null);

            Transform zombieTransform = spawnedZombieObject.transform;
            Zombie zombie = spawnedZombieObject.GetComponent<Zombie>();
            zombieTransform.SetParent(null, true);

            zombie.StateMachine.Enter<NavigationState>();
            zombie.Awakener.Awake();

            if (++_spawnedCount == _spawnCount)
                Object.Destroy(_gameObject);
        }

        private Quaternion GetRotation()
        {
            Vector3 targetPosition = _closestTargetObserver.Trigger.Value.Transform.position;
            Vector3 currentPosition = _transform.position;
            targetPosition.y = currentPosition.y;
            Vector3 direction = targetPosition - currentPosition;
            return Quaternion.LookRotation(direction);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private RangeDependentValue _interval;
            [SerializeField] private RangeDependentValue _count;

            public RangeDependentValue Interval => _interval;
            public RangeDependentValue Count => _count;
        }
    }
}