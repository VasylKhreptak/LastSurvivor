using System;
using Gameplay.Weapons;
using Gameplay.Weapons.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Gameplay.ProgressBars
{
    public class WeaponReloadProgressBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _image;

        private WeaponHolder _weaponHolder;

        [Inject]
        private void Constructor(WeaponHolder weaponHolder)
        {
            _weaponHolder = weaponHolder;
        }

        private readonly int _propertyName = Shader.PropertyToID("_Arc2");

        private Material _material;

        private IDisposable _weaponSubscription;
        private IDisposable _progressSubscription;

        #region MoonBehaviour

        private void Awake() => _material = _image.materialForRendering;

        private void OnValidate() => _image ??= GetComponent<Image>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => StartObservingWeapon();

        private void StopObserving()
        {
            StopObservingWeapon();
            StopObservingProgress();
        }

        private void StartObservingWeapon() => _weaponSubscription = _weaponHolder.Instance.Subscribe(OnWeaponChanged);

        private void StopObservingWeapon() => _weaponSubscription?.Dispose();

        private void OnWeaponChanged(IWeapon weapon)
        {
            StopObservingProgress();

            if (weapon == null)
                return;

            StartObservingProgress(weapon.ReloadProgress);
        }

        private void StartObservingProgress(IReadOnlyReactiveProperty<float> progress) =>
            _progressSubscription = progress.Subscribe(SetProgress);

        private void StopObservingProgress() => _progressSubscription?.Dispose();

        private void SetProgress(float progress)
        {
            if (Mathf.Approximately(progress, 0f))
                return;

            _material.SetFloat(_propertyName, (1 - progress) * 360);
        }
    }
}