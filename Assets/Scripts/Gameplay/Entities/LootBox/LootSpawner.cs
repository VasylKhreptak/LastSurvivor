using System;
using System.Collections.Generic;
using Extensions;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Entities.LootBox
{
    public class LootSpawner
    {
        private readonly IObjectPools<GeneralPool> _generalPools;
        private readonly Preferences _preferences;

        public LootSpawner(IObjectPools<GeneralPool> generalPools, Preferences preferences)
        {
            _generalPools = generalPools;
            _preferences = preferences;
        }

        public void Spawn()
        {
            foreach (LootPreference lootPreferences in _preferences.LootPreferences)
            {
                for (int i = 0; i < Random.Range(lootPreferences.MinInstanceCount, lootPreferences.MaxInstanceCount); i++)
                {
                    GameObject lootObject = _generalPools.GetPool(lootPreferences.Pool).Get();
                    lootObject.transform.position = GetPosition();
                    lootObject.transform.rotation = GetRotation();
                    Loot.Variety.Core.Loot loot = lootObject.GetComponent<Loot.Variety.Core.Loot>();
                    loot.Rigidbody.velocity = GetVelocity();
                    loot.Data.Count = Random.Range(lootPreferences.MinCount, lootPreferences.MaxCount);
                }
            }
        }

        private Vector3 GetPosition() => _preferences.Origin.position + _preferences.SpawnRadius * Random.insideUnitSphere;

        private Quaternion GetRotation() =>
            Quaternion.Euler(RandomExtensions.Range(_preferences.MinStartAngle, _preferences.MaxStartAngle));

        private Vector3 GetVelocity()
        {
            Vector3 direction = _preferences.Direction;

            direction = Quaternion.Euler(RandomExtensions.Range(Vector3.one * _preferences.MinAngleDeviation,
                            Vector3.one * _preferences.MaxAngleDeviation)) *
                        direction;

            return direction * Random.Range(_preferences.MinVelocity, _preferences.MaxVelocity);
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _origin;
            [SerializeField] private List<LootPreference> _lootPreferences;
            [SerializeField] private float _spawnRadius = 0.3f;
            [SerializeField] private Vector3 _direction = Vector3.up;
            [SerializeField] private Vector3 _minStartAngle;
            [SerializeField] private Vector3 _maxStartAngle;
            [SerializeField] private float _minAngleDeviation;
            [SerializeField] private float _maxAngleDeviation = 20f;
            [SerializeField] private float _minVelocity = 3f;
            [SerializeField] private float _maxVelocity = 10f;

            public Transform Origin => _origin;
            public IReadOnlyList<LootPreference> LootPreferences => _lootPreferences;
            public float SpawnRadius => _spawnRadius;
            public Vector3 Direction => _direction;
            public Vector3 MinStartAngle => _minStartAngle;
            public Vector3 MaxStartAngle => _maxStartAngle;
            public float MinAngleDeviation => _minAngleDeviation;
            public float MaxAngleDeviation => _maxAngleDeviation;
            public float MinVelocity => _minVelocity;
            public float MaxVelocity => _maxVelocity;
        }

        [Serializable]
        public class LootPreference
        {
            [SerializeField] private GeneralPool _pool = GeneralPool.GearLoot;
            [SerializeField] private int _minInstanceCount;
            [SerializeField] private int _maxInstanceCount;
            [SerializeField] private int _minCount;
            [SerializeField] private int _maxCount;

            public GeneralPool Pool => _pool;
            public int MinInstanceCount => _minInstanceCount;
            public int MaxInstanceCount => _maxInstanceCount;
            public int MinCount => _minCount;
            public int MaxCount => _maxCount;
        }
    }
}