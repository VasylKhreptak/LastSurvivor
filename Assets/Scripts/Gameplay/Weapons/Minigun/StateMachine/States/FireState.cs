using System;
using CameraUtilities.Shaker;
using Extensions;
using Gameplay.Weapons.Bullets.Core;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class FireState : IMinigunState, IExitable
    {
        private readonly Preferences _preferences;
        private readonly IObjectPools<GeneralPool> _generalPools;
        private readonly CameraShaker _cameraShaker;
        private readonly IObjectPools<Particle> _particlePools;

        public FireState(Preferences preferences, IObjectPools<GeneralPool> generalPools, CameraShaker cameraShaker,
            IObjectPools<Particle> particlePools)
        {
            _preferences = preferences;
            _generalPools = generalPools;
            _cameraShaker = cameraShaker;
            _particlePools = particlePools;
        }

        private IDisposable _fireSubscription;

        private Vector3 _lastBulletPosition;

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
            SpawnShootParticle();
            FireShell();
            ShakeCamera();
        }

        private void FireBullet()
        {
            GameObject bulletObject = _generalPools.GetPool(GeneralPool.Bullet).Get();
            IBullet bullet = bulletObject.GetComponent<IBullet>();

            Vector3 bulletPosition = GetBulletPosition();
            _lastBulletPosition = bulletPosition;
            bulletObject.transform.position = bulletPosition;
            bulletObject.transform.rotation = GetBulletRotation();
            bullet.Damage = GetDamage();
            AccelerateBullet(bullet);
        }

        private void SpawnShootParticle()
        {
            GameObject particle = _particlePools.GetPool(Particle.Shoot).Get();
            particle.transform.position = _lastBulletPosition;
            particle.transform.forward = _preferences.Bullet.SpawnPoint.forward;
        }

        private void FireShell()
        {
            GameObject shellObject = _generalPools.GetPool(GeneralPool.BulletShell).Get();

            shellObject.transform.position = _preferences.Shell.SpawnPoint.position;
            shellObject.transform.rotation = _preferences.Shell.SpawnPoint.rotation;

            Rigidbody rigidbody = shellObject.GetComponent<Rigidbody>();

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            rigidbody.AddRelativeTorque(RandomExtensions.Sign() *
                                        _preferences.Shell.TorqueAxis *
                                        Random.Range(_preferences.Shell.MinTorque, _preferences.Shell.MaxTorque));
            rigidbody.velocity = shellObject.transform.right * Random.Range(_preferences.Shell.MinVelocity, _preferences.Shell.MaxVelocity);
        }

        private void ShakeCamera() => _cameraShaker.DoFireShake();

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
                [SerializeField] private Vector3 _torqueAxis = Vector3.one;
                [SerializeField] private float _minTorque = 0.5f;
                [SerializeField] private float _maxTorque = 1.5f;
                [SerializeField] private float _minVelocity = 0.5f;
                [SerializeField] private float _maxVelocity = 1.5f;

                public Transform SpawnPoint => _spawnPoint;
                public Vector3 TorqueAxis => _torqueAxis;
                public float MinTorque => _minTorque;
                public float MaxTorque => _maxTorque;
                public float MinVelocity => _minVelocity;
                public float MaxVelocity => _maxVelocity;
            }
        }
    }
}