using System;
using Gameplay.Weapons.Bullets.Core;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Plugins.ObjectPoolSystem;
using Plugins.ObjectPoolSystem.Test;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class FireState : IMinigunState, IExitable
    {
        private readonly Preferences _preferences;
        private readonly IObjectPools<MainPool> _objectPools;

        public FireState(Preferences preferences, IObjectPools<MainPool> objectPools)
        {
            _preferences = preferences;
            _objectPools = objectPools;
        }

        private IDisposable _fireSubscription;

        public void Enter() => StartFiring();

        public void Exit() => StopFiring();

        private void StartFiring()
        {
            StopFiring();
            _fireSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.Bullet.FireInterval))
                .DoOnSubscribe(Fire)
                .Subscribe(_ => Fire());
        }

        private void StopFiring() => _fireSubscription?.Dispose();

        private void Fire()
        {
            FireBullet();
            FireShell();
        }

        private void FireBullet()
        {
            GameObject bulletObject = _objectPools.GetPool(MainPool.Bullet).Get();
            IBullet bullet = bulletObject.GetComponent<IBullet>();

            bulletObject.transform.position = GetBulletPosition();
            bulletObject.transform.rotation = GetBulletRotation();
            bullet.Damage = GetDamage();
            AccelerateBullet(bullet);
        }

        private void FireShell() { }

        private Vector3 GetBulletPosition()
        {
            Vector3 position = _preferences.Bullet.SpawnPoint.position;

            position += _preferences.Bullet.SpawnPoint.right *
                        Random.Range(-_preferences.Bullet.MaxLocalSpawnOffset.x, _preferences.Bullet.MaxLocalSpawnOffset.x);
            position += _preferences.Bullet.SpawnPoint.up *
                        Random.Range(-_preferences.Bullet.MaxLocalSpawnOffset.y, _preferences.Bullet.MaxLocalSpawnOffset.y);

            return position;
        }

        private Quaternion GetBulletRotation()
        {
            Quaternion rotation = _preferences.Bullet.SpawnPoint.rotation;

            rotation *= Quaternion.Euler(Random.Range(_preferences.Bullet.MinScatterAngle, _preferences.Bullet.MaxScatterAngle),
                Random.Range(_preferences.Bullet.MinScatterAngle, _preferences.Bullet.MaxScatterAngle),
                Random.Range(_preferences.Bullet.MinScatterAngle, _preferences.Bullet.MaxScatterAngle));

            return rotation;
        }

        private float GetDamage() => Random.Range(_preferences.Bullet.MinDamage, _preferences.Bullet.MaxDamage);

        private void AccelerateBullet(IBullet bullet)
        {
            bullet.Rigidbody.angularVelocity = Vector3.zero;
            bullet.Rigidbody.velocity = bullet.Transform.forward * _preferences.Bullet.Velocity;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private BulletBehaviour _bullet;
            [SerializeField] private ShellBehaviour _shell;

            public BulletBehaviour Bullet => _bullet;
            public ShellBehaviour Shell => _shell;

            [Serializable]
            public class BulletBehaviour
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

            [Serializable]
            public class ShellBehaviour
            {
                [SerializeField] private Transform _spawnPoint;
                [SerializeField] private float _minTorque = 0.5f;
                [SerializeField] private float _maxTorque = 1.5f;
                [SerializeField] private float _minForce = 0.5f;
                [SerializeField] private float _maxForce = 1.5f;

                public Transform SpawnPoint => _spawnPoint;
                public float MinTorque => _minTorque;
                public float MaxTorque => _maxTorque;
                public float MinForce => _minForce;
                public float MaxForce => _maxForce;
            }
        }
    }
}