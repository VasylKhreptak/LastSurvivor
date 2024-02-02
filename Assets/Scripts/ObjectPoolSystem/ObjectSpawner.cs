using System;
using Plugins.ObjectPoolSystem;
using UnityEngine;

namespace ObjectPoolSystem
{
    public class ObjectSpawner<T> where T : Enum
    {
        private readonly IObjectPools<T> _objectPools;
        private readonly Preferences _preferences;

        public ObjectSpawner(IObjectPools<T> objectPools, Preferences preferences)
        {
            _objectPools = objectPools;
            _preferences = preferences;
        }

        public GameObject Spawn(Vector3 position) => Spawn(position, Quaternion.identity);

        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject obj = _objectPools.GetPool(_preferences.PoolType).Get();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private T _poolType;

            public T PoolType => _poolType;
        }
    }
}