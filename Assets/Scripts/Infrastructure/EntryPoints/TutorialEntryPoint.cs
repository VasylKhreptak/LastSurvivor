using Gameplay.Weapons;
using Gameplay.Weapons.Core;
using Gameplay.Weapons.Minigun;
using Infrastructure.EntryPoints.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class TutorialEntryPoint : MonoBehaviour, IEntryPoint
    {
        private WeaponHolder _weaponHolder;
        private DiContainer _container;
        private DisposableManager _disposableManager;

        [Inject]
        private void Constructor(WeaponHolder weaponHolder, DiContainer container, DisposableManager disposableManager)
        {
            _weaponHolder = weaponHolder;
            _container = container;
            _disposableManager = disposableManager;
        }

        #region MonoBehaviour

        private void Start() => Enter();

        #endregion

        public void Enter()
        {
            InitializeWeapon();
        }

        private void InitializeWeapon()
        {
            IWeapon weapon = FindObjectOfType<Minigun>();
            _weaponHolder.Instance = weapon;

            WeaponShooter weaponShooter = _container.Instantiate<WeaponShooter>();
            weaponShooter.Initialize();
            _disposableManager.Add(weaponShooter);
            _container.BindInstance(weaponShooter).AsSingle();
        }
    }
}