using System;
using System.Collections.Generic;
using Gameplay.Aim;
using Gameplay.Data;
using Gameplay.Entities.Helicopter;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Gameplay.Waypoints;
using Gameplay.Weapons;
using Levels.StateMachine;
using Levels.StateMachine.States;
using Levels.StateMachine.States.Core;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UnityEngine;
using Utilities.CameraUtilities.Shaker;

namespace Zenject.Installers.SceneContext.Gameplay
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Camera _camera;
        [SerializeField] private Trackpad _trackpad;

        [Header("Preferences")]
        [SerializeField] private CameraShaker.Preferences _cameraShakerPreferences;
        [SerializeField] private Transform[] _playerWaypoints;

        [Header("Object Pool Preferences")]
        [SerializeField] private ObjectPoolPreference<Particle>[] _particlePoolsPreferences;
        [SerializeField] private ObjectPoolPreference<GeneralPool>[] _generalPoolsPreferences;

        [Header("Weapons")]
        [SerializeField] private WeaponAimer.Preferences _weaponAimPreferences;

        [Header("Entities")]
        [SerializeField] private Helicopter _helicopter;

        #region MonoBehaviour

        private void OnValidate()
        {
            _camera ??= FindObjectOfType<Camera>(true);
            _trackpad ??= FindObjectOfType<Trackpad>(true);
            _helicopter ??= FindObjectOfType<Helicopter>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_camera).AsSingle();
            Container.BindInstance(_trackpad).AsSingle();
            Container.BindInstance(_helicopter).AsSingle();

            BindHolders();
            BindCameraShaker();
            BindPlayerWaypoints();
            BindObjectPools();
            BindZombieList();
            BindLevelData();
            BindWeaponAimer();
            BindWeaponShooter();
            BindLevelStateMachine();
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

        private void BindLevelStateMachine()
        {
            BindLevelStates();
            Container.Bind<LevelStateFactory>().AsSingle();
            Container.BindInterfacesTo<LevelStateMachine>().AsSingle();
        }

        private void BindLevelStates()
        {
            Container.Bind<LevelStartState>().AsSingle();
            Container.Bind<LevelCompletedState>().AsSingle();
            Container.Bind<LevelFailedState>().AsSingle();
            Container.Bind<LevelLoopState>().AsSingle();
        }

        private void BindZombieList() => Container.Bind<List<Zombie>>().AsSingle();

        private void BindLevelData() => Container.Bind<LevelData>().AsSingle();

        private void BindWeaponAimer() =>
            Container.BindInterfacesAndSelfTo<WeaponAimer>().AsSingle().WithArguments(_weaponAimPreferences);

        private void BindWeaponShooter() => Container.BindInterfacesAndSelfTo<WeaponShooter>().AsSingle();
    }
}