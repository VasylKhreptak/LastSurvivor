using System;
using Gameplay.Weapons.Core;
using TMPro;
using UI.Texts.Core;
using UnityEngine;
using Zenject;

namespace UI.Gameplay.Texts
{
    public class WeaponAmmoText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        private IWeapon _weapon;

        [Inject]
        private void Constructor(IWeapon weapon)
        {
            _weapon = weapon;
        }

        private PropertyTextConverter<int> _propertyTextConverter;

        private IDisposable _weaponSubscription;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable()
        {
            _propertyTextConverter = new PropertyTextConverter<int>(_tmp, _weapon.Ammo.Value);
            _propertyTextConverter.Initialize();
        }

        private void OnDisable() => _propertyTextConverter?.Dispose();

        #endregion
    }
}