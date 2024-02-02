using System;
using Audio.Players;
using Extensions;
using Gameplay.Weapons.Bullets;
using Gameplay.Weapons.Core.Fire;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Holders.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using ObjectPoolSystem;
using ObjectPoolSystem.PoolCategories;
using Plugins.Banks;
using Plugins.ObjectPoolSystem;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class ShootState : IMinigunState, IPayloadedState<InstanceHolder<Action>>, IExitable
    {
        private readonly Preferences _preferences;
        private readonly IObjectPools<GeneralPool> _generalPools;
        private readonly ObjectSpawner<Particle> _shootParticleSpawner;
        private readonly ShellSpawner _shellSpawner;
        private readonly AudioPlayer _shootAudioPlayer;
        private readonly ClampedIntegerBank _ammo;
        private readonly IStateMachine<IMinigunState> _stateMachine;

        public ShootState(Preferences preferences, IObjectPools<GeneralPool> generalPools,
            ObjectSpawner<Particle> shootParticleSpawner,
            ShellSpawner shellSpawner, AudioPlayer shootAudioPlayer, ClampedIntegerBank ammo,
            IStateMachine<IMinigunState> stateMachine)
        {
            _preferences = preferences;
            _generalPools = generalPools;
            _shootParticleSpawner = shootParticleSpawner;
            _shellSpawner = shellSpawner;
            _shootAudioPlayer = shootAudioPlayer;
            _ammo = ammo;
            _stateMachine = stateMachine;
        }

        private IDisposable _shootSubscription;

        private InstanceHolder<Action> _reloadStatePayload;

        private Vector3 _bulletPosition;
        private Quaternion _bulletRotation;

        public void Enter(InstanceHolder<Action> payload = null)
        {
            _reloadStatePayload = payload;

            StartShooting();
        }

        public void Exit() => StopShooting();

        private void StartShooting()
        {
            StopShooting();
            _shootSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.FireInterval))
                .DoOnSubscribe(TryShoot)
                .Subscribe(_ => TryShoot());
        }

        private void StopShooting() => _shootSubscription?.Dispose();

        private void TryShoot()
        {
            if (_ammo.Spend(1))
                Shoot();

            if (_ammo.HasEnough(1))
                return;

            _stateMachine.Enter<ReloadState, InstanceHolder<Action>>(_reloadStatePayload);
        }

        private void Shoot()
        {
            SpawnBullet();
            _shootParticleSpawner.Spawn(_bulletPosition, _bulletRotation);
            _shellSpawner.Spawn();
            _shootAudioPlayer.Play(_bulletPosition);
        }

        private void SpawnBullet()
        {
            GameObject bulletObject = _generalPools.GetPool(GeneralPool.Bullet).Get();
            Bullet bullet = bulletObject.GetComponent<Bullet>();

            Vector3 bulletPosition = GetBulletPosition();
            bulletObject.transform.position = bulletPosition;
            bulletObject.transform.rotation = GetBulletRotation();
            bullet.Damage.Value = GetDamage();
            _bulletPosition = bulletPosition;
            _bulletRotation = bulletObject.transform.rotation;
            AccelerateBullet(bullet);
        }

        private Vector3 GetBulletPosition()
        {
            Vector3 position = _preferences.SpawnPoint.position;

            position += _preferences.SpawnPoint.right *
                        Random.Range(-_preferences.MaxLocalSpawnOffset.x, _preferences.MaxLocalSpawnOffset.x);
            position += _preferences.SpawnPoint.up *
                        Random.Range(-_preferences.MaxLocalSpawnOffset.y, _preferences.MaxLocalSpawnOffset.y);

            return position;
        }

        private Quaternion GetBulletRotation()
        {
            Quaternion rotation = _preferences.SpawnPoint.rotation;

            rotation *= Quaternion.Euler(RandomExtensions.Range(Vector3.one * _preferences.MinScatterAngle,
                Vector3.one * _preferences.MaxScatterAngle));

            return rotation;
        }

        private float GetDamage() => Random.Range(_preferences.MinDamage, _preferences.MaxDamage);

        private void AccelerateBullet(Bullet bullet)
        {
            bullet.Rigidbody.angularVelocity = Vector3.zero;
            bullet.Rigidbody.velocity = bullet.Transform.forward * _preferences.Velocity;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _spawnPoint;
            [SerializeField] private float _fireInterval = 0.09f;
            [SerializeField] private Vector3 _maxLocalSpawnOffset;
            [SerializeField] private float _minScatterAngle = 1;
            [SerializeField] private float _maxScatterAngle = 5;
            [SerializeField] private float _minDamage = 5f;
            [SerializeField] private float _maxDamage = 20f;
            [SerializeField] private float _velocity = 20f;

            public Transform SpawnPoint => _spawnPoint;
            public float FireInterval => _fireInterval;
            public Vector3 MaxLocalSpawnOffset => _maxLocalSpawnOffset;
            public float MinScatterAngle => _minScatterAngle;
            public float MaxScatterAngle => _maxScatterAngle;
            public float MinDamage => _minDamage;
            public float MaxDamage => _maxDamage;
            public float Velocity => _velocity;
        }
    }
}