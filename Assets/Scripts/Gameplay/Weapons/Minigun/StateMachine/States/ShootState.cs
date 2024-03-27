using System;
using Audio.Players;
using Extensions;
using Gameplay.Weapons.Bullets;
using Gameplay.Weapons.Core.Fire;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Inspector.MinMax;
using ObjectPoolSystem;
using ObjectPoolSystem.PoolCategories;
using Plugins.Banks;
using Plugins.ObjectPoolSystem;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class ShootState : IMinigunState, IPayloadedState<Action>, IExitable
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

        private Action _reloadCallback;

        private IDisposable _shootSubscription;

        private Vector3 _bulletPosition;
        private Quaternion _bulletRotation;

        public void Enter(Action onReloaded = null)
        {
            _reloadCallback = onReloaded;
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

            StopShooting();

            _stateMachine.Enter<ReloadState, Action>(_reloadCallback);
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
            bullet.Damage.Value = _preferences.Damage.GetRandom();
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

            rotation *= Quaternion.Euler(RandomExtensions.Range(Vector3.one * _preferences.ScatterAngle.Min,
                Vector3.one * _preferences.ScatterAngle.Max));

            return rotation;
        }

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
            [SerializeField] private FloatMinMaxValue _scatterAngle = new FloatMinMaxValue(0.1f, 1.5f);
            [SerializeField] private FloatMinMaxValue _damage = new FloatMinMaxValue(5f, 15f);
            [SerializeField] private float _velocity = 20f;

            public Transform SpawnPoint => _spawnPoint;
            public float FireInterval => _fireInterval;
            public Vector3 MaxLocalSpawnOffset => _maxLocalSpawnOffset;
            public FloatMinMaxValue ScatterAngle => _scatterAngle;
            public FloatMinMaxValue Damage => _damage;
            public float Velocity => _velocity;
        }
    }
}