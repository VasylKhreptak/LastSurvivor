using System;
using Gameplay.Aim;
using Gameplay.Data;
using Gameplay.Entities.Player;
using Gameplay.Waypoints;
using Gameplay.Weapons;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UnityEngine;
using Utilities.CameraUtilities.Shaker;

namespace Zenject.Installers.SceneContext.Gameplay
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;
        [SerializeField] private Trackpad _trackpad;

        [Header("Preferences")]
        [SerializeField] private CameraShaker.Preferences _cameraShakerPreferences;
        [SerializeField] private Transform[] _playerWaypoints;

        [Header("Object Pool Preferences")]
        [SerializeField] private ObjectPoolPreference<Particle>[] _particlePoolsPreferences;
        [SerializeField] private ObjectPoolPreference<GeneralPool>[] _generalPoolsPreferences;

        #region MonoBehaviour

        private void OnValidate()
        {
            _canvas ??= FindObjectOfType<Canvas>(true);
            _camera ??= FindObjectOfType<Camera>(true);
            _trackpad ??= FindObjectOfType<Trackpad>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_canvas).AsSingle();
            Container.BindInstance(_camera).AsSingle();
            Container.BindInstance(_trackpad).AsSingle();

            BindHolders();
            BindCameraShaker();
            BindPlayerWaypoints();
            BindPlayerWaypointsFollower();
            BindObjectPools();
        }

        private void BindHolders()
        {
            Container.Bind<WeaponHolder>().AsSingle();
            Container.Bind<PlayerHolder>().AsSingle();
        }

        private void BindCameraShaker()
        {
            Container.BindInterfacesAndSelfTo<CameraShaker>().AsSingle().WithArguments(_cameraShakerPreferences);
        }

        private void BindPlayerWaypoints() => Container.Bind<PlayerWaypoints>().AsSingle().WithArguments(_playerWaypoints);

        private void BindPlayerWaypointsFollower() => Container.Bind<PlayerWaypointFollower>().AsSingle();

        private void BindObjectPools()
        {
            BindObjectPools(_particlePoolsPreferences, new GameObject("Particles").transform);
            BindObjectPools(_generalPoolsPreferences, new GameObject("General Pool").transform);
        }

        private void BindObjectPools<T>(ObjectPoolPreference<T>[] poolPreferences, Transform parent) where T : Enum
        {
            foreach (ObjectPoolPreference<T> preference in poolPreferences)
            {
                preference.CreateFunc = () => Container.InstantiatePrefab(preference.Prefab, parent);
            }

            ObjectPools<T> objectPools = new ObjectPools<T>(poolPreferences);

            Container.Bind<IObjectPools<T>>().FromInstance(objectPools).AsSingle();
        }
    }
}