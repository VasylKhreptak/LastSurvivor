using System;
using Audio.Players;
using Extensions;
using Gameplay.Weapons.Bullets.Core;
using Gameplay.Weapons.Core.Fire;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using ObjectPoolSystem.PoolCategories;
using Plugins.Banks;
using Plugins.ObjectPoolSystem;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class ShootState : IMinigunState, IExitable
    {
        private readonly Preferences _preferences;
        private readonly IObjectPools<GeneralPool> _generalPools;
        private readonly ShootParticle _shootParticle;
        private readonly ShellSpawner _shellSpawner;
        private readonly AudioPlayer _shootAudioPlayer;
        private readonly ClampedIntegerBank _ammo;

        public ShootState(Preferences preferences, IObjectPools<GeneralPool> generalPools, ShootParticle shootParticle,
            ShellSpawner shellSpawner, AudioPlayer shootAudioPlayer, ClampedIntegerBank ammo)
        {
            _preferences = preferences;
            _generalPools = generalPools;
            _shootParticle = shootParticle;
            _shellSpawner = shellSpawner;
            _shootAudioPlayer = shootAudioPlayer;
            _ammo = ammo;
        }

        private IDisposable _fireSubscription;

        private Vector3 _bulletPosition;
        private Vector3 _bulletDirection;

        public void Enter() => StartFiring();

        public void Exit() => StopFiring();

        private void StartFiring()
        {
            StopFiring();
            _fireSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.FireInterval))
                .DoOnSubscribe(Fire)
                .Subscribe(_ => Fire());
        }

        private void StopFiring() => _fireSubscription?.Dispose();

        private void Fire()
        {
            SpawnBullet();
            _shootParticle.Spawn(_bulletPosition, _bulletDirection);
            _shellSpawner.Spawn();
            _shootAudioPlayer.Play(_bulletPosition);
        }

        private void SpawnBullet()
        {
            GameObject bulletObject = _generalPools.GetPool(GeneralPool.Bullet).Get();
            IBullet bullet = bulletObject.GetComponent<IBullet>();

            Vector3 bulletPosition = GetBulletPosition();
            bulletObject.transform.position = bulletPosition;
            bulletObject.transform.rotation = GetBulletRotation();
            bullet.Damage.Value = GetDamage();
            _bulletPosition = bulletPosition;
            _bulletDirection = bulletObject.transform.forward;
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

        private void AccelerateBullet(IBullet bullet)
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