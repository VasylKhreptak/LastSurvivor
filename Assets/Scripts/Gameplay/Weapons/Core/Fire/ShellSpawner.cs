using System;
using Extensions;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using Providers.Velocity.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Weapons.Core.Fire
{
    public class ShellSpawner
    {
        private readonly IObjectPools<GeneralPool> _generalPools;
        private readonly IVelocityProvider _velocityProvider;
        private readonly Preferences _preferences;

        public ShellSpawner(IObjectPools<GeneralPool> generalPools, IVelocityProvider velocityProvider,
            Preferences preferences)
        {
            _generalPools = generalPools;
            _velocityProvider = velocityProvider;
            _preferences = preferences;
        }

        public void Spawn()
        {
            GameObject shellObject = _generalPools.GetPool(_preferences.Shell).Get();

            shellObject.transform.position = _preferences.SpawnPoint.position;
            shellObject.transform.rotation = _preferences.SpawnPoint.rotation;

            Rigidbody rigidbody = shellObject.GetComponent<Rigidbody>();

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            rigidbody.AddRelativeTorque(RandomExtensions.Sign() *
                                        _preferences.TorqueAxis *
                                        Random.Range(_preferences.MinTorque, _preferences.MaxTorque));
            rigidbody.velocity = shellObject.transform.right *
                                 Random.Range(_preferences.MinVelocity, _preferences.MaxVelocity) +
                                 _velocityProvider.Value;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private GeneralPool _shell = GeneralPool.BulletShell;
            [SerializeField] private Transform _spawnPoint;
            [SerializeField] private Vector3 _torqueAxis = Vector3.one;
            [SerializeField] private float _minTorque = 30f;
            [SerializeField] private float _maxTorque = 90f;
            [SerializeField] private float _minVelocity = 1.5f;
            [SerializeField] private float _maxVelocity = 3f;

            public GeneralPool Shell => _shell;
            public Transform SpawnPoint => _spawnPoint;
            public Vector3 TorqueAxis => _torqueAxis;
            public float MinTorque => _minTorque;
            public float MaxTorque => _maxTorque;
            public float MinVelocity => _minVelocity;
            public float MaxVelocity => _maxVelocity;
        }
    }
}