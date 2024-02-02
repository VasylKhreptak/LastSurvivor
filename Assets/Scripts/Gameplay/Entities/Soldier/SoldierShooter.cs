using System;
using Extensions;
using Gameplay.Weapons.Bullets;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Entities.Soldier
{
    public class SoldierShooter
    {
        private readonly SoldierAimer _aimer;
        private readonly IObjectPools<GeneralPool> _generalPools;
        private readonly Preferences _preferences;

        public SoldierShooter(SoldierAimer aimer, IObjectPools<GeneralPool> generalPools, Preferences preferences)
        {
            _aimer = aimer;
            _generalPools = generalPools;
            _preferences = preferences;
        }

        private IDisposable _isAimedSubscription;
        private IDisposable _shootSubscription;

        private Vector3 _bulletDirection;

        public void Enable()
        {
            StartObservingAim();
        }

        public void Disable()
        {
            StopObservingAim();
            StopShooting();
        }

        private void StartObservingAim() => _isAimedSubscription = _aimer.IsAimed.Subscribe(OnIsAimedChanged);

        private void StopObservingAim() => _isAimedSubscription?.Dispose();

        private void OnIsAimedChanged(bool isAimed)
        {
            if (isAimed)
                StartShooting();
            else
                StopShooting();
        }

        private void StartShooting()
        {
            _shootSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.FireInterval))
                .Delay(TimeSpan.FromSeconds(GetShootDelay()))
                .DoOnSubscribe(Shoot)
                .Subscribe(_ => Shoot());
        }

        private void StopShooting() => _shootSubscription?.Dispose();

        private void Shoot()
        {
            SpawnBullet();
        }

        private void SpawnBullet()
        {
            GameObject bulletObject = _generalPools.GetPool(GeneralPool.SmallBullet).Get();
            Bullet bullet = bulletObject.GetComponent<Bullet>();

            bulletObject.transform.position = _preferences.SpawnPoint.position;
            bulletObject.transform.rotation = GetBulletRotation();
            bullet.Damage.Value = GetDamage();
            _bulletDirection = bulletObject.transform.forward;
            AccelerateBullet(bullet);
        }

        private Quaternion GetBulletRotation()
        {
            Quaternion rotation = _preferences.SpawnPoint.rotation;

            rotation *= Quaternion.Euler(RandomExtensions.Range(Vector3.one * _preferences.MinScatterAngle,
                Vector3.one * _preferences.MaxScatterAngle));

            return rotation;
        }

        private float GetDamage() => Random.Range(_preferences.MinDamage, _preferences.MaxDamage);

        private float GetShootDelay() => Random.Range(_preferences.MinShootDelay, _preferences.MaxShootDelay);

        private void AccelerateBullet(Bullet bullet)
        {
            bullet.Rigidbody.angularVelocity = Vector3.zero;
            bullet.Rigidbody.velocity = bullet.Transform.forward * _preferences.Velocity;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _spawnPoint;
            [SerializeField] private float _fireInterval = 0.15f;
            [SerializeField] private float _minScatterAngle = 1;
            [SerializeField] private float _maxScatterAngle = 5;
            [SerializeField] private float _minDamage = 5f;
            [SerializeField] private float _maxDamage = 20f;
            [SerializeField] private float _velocity = 20f;
            [SerializeField] private float _minShootDelay = 0.1f;
            [SerializeField] private float _maxShootDelay = 1f;

            public Transform SpawnPoint => _spawnPoint;
            public float FireInterval => _fireInterval;
            public float MinScatterAngle => _minScatterAngle;
            public float MaxScatterAngle => _maxScatterAngle;
            public float MinDamage => _minDamage;
            public float MaxDamage => _maxDamage;
            public float Velocity => _velocity;
            public float MinShootDelay => _minShootDelay;
            public float MaxShootDelay => _maxShootDelay;
        }
    }
}