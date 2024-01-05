using System;
using UnityEngine;
using Zenject;

namespace Plugins.ObjectPoolSystem.Zenject
{
    public class ObjectPoolsInstaller<T> : MonoInstaller where T : Enum
    {
        [Header("Preferences")]
        [SerializeField] private ObjectPoolPreference<T>[] _objectPoolPreferences;

        public override void InstallBindings()
        {
            foreach (ObjectPoolPreference<T> preference in _objectPoolPreferences)
            {
                preference.CreateFunc = () => Container.InstantiatePrefab(preference.Prefab, transform);
            }

            ObjectPools<T> objectPools = new ObjectPools<T>(_objectPoolPreferences);

            Container.Bind<IObjectPools<T>>().FromInstance(objectPools).AsSingle();
        }
    }
}