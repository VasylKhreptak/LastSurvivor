using System;
using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UniRx;
using UnityEngine;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class FireState : IMinigunState, IExitable
    {
        private readonly Preferences _preferences;

        public FireState(Preferences preferences)
        {
            _preferences = preferences;
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
            Debug.Log("Fire!");
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

                public Transform SpawnPoint => _spawnPoint;
                public float FireInterval => _fireInterval;
                public Vector3 MaxLocalSpawnOffset => _maxLocalSpawnOffset;
                public float MinScatterAngle => _minScatterAngle;
                public float MaxScatterAngle => _maxScatterAngle;
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