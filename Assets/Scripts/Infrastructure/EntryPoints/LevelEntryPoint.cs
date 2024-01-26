using Gameplay.Entities.Player;
using Gameplay.Weapons;
using Gameplay.Weapons.Core;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.EntryPoints.Core;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class LevelEntryPoint : MonoBehaviour, IEntryPoint
    {
        private WeaponHolder _weaponHolder;
        private DiContainer _container;
        private DisposableManager _disposableManager;
        private PlayerHolder _playerHolder;
        private GamePrefabs _gamePrefabs;

        [Inject]
        private void Constructor(WeaponHolder weaponHolder, DiContainer container, DisposableManager disposableManager,
            PlayerHolder playerHolder, IStaticDataService staticDataService)
        {
            _weaponHolder = weaponHolder;
            _container = container;
            _disposableManager = disposableManager;
            _playerHolder = playerHolder;
            _gamePrefabs = staticDataService.Prefabs;
        }

        #region MonoBehaviour

        private void Start() => Enter();

        #endregion

        public void Enter()
        {
            InitializeWeapon();
            InitializePlayer();
        }

        private void InitializeWeapon()
        {
            IWeapon weapon = FindObjectOfType<Weapon>();
            _weaponHolder.Instance = weapon;

            WeaponShooter weaponShooter = _container.Instantiate<WeaponShooter>();
            weaponShooter.Initialize();
            _disposableManager.Add(weaponShooter);
            _container.BindInstance(weaponShooter).AsSingle();
        }

        private void InitializePlayer()
        {
            GameObject playerObject = _container.InstantiatePrefab(_gamePrefabs[Prefab.GameplayPlayer]);
            Player player = playerObject.GetComponent<Player>();
            _playerHolder.Instance = player;
        }
    }
}