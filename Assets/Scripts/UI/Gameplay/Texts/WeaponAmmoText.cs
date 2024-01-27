using System;
using Gameplay.Weapons;
using Gameplay.Weapons.Core;
using TMPro;
using UI.Texts.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Gameplay.Texts
{
    public class WeaponAmmoText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        private WeaponHolder _weaponHolder;

        [Inject]
        private void Constructor(WeaponHolder weaponHolder)
        {
            _weaponHolder = weaponHolder;
        }

        private PropertyTextConverter<int> _propertyTextConverter;

        private IDisposable _weaponSubscription;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable()
        {
            StartObservingWeapon();
        }

        private void OnDisable()
        {
            StopObservingWeapon();
            _propertyTextConverter?.Dispose();
        }

        #endregion

        private void StartObservingWeapon() => _weaponSubscription = _weaponHolder.Instance.Subscribe(OnWeaponChanged);

        private void StopObservingWeapon() => _weaponSubscription?.Dispose();

        private void OnWeaponChanged(IWeapon weapon)
        {
            if (weapon == null)
            {
                _propertyTextConverter?.Dispose();
                return;
            }

            if (_propertyTextConverter == null)
            {
                _propertyTextConverter = new PropertyTextConverter<int>(_tmp, weapon.Ammo.Value);
                _propertyTextConverter.Initialize();
            }

            _propertyTextConverter.SetObservable(weapon.Ammo.Value);
        }
    }
}