using System;
using Gameplay.Weapons;
using Gameplay.Weapons.Core;
using Infrastructure.Graphics.UI.Windows.Core;
using Plugins.Animations;
using Plugins.Animations.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Gameplay.Windows
{
    public class WeaponReloadProgress : MonoBehaviour, IWindow
    {
        [Header("References")]
        [SerializeField] private GameObject _aim;
        [SerializeField] private GameObject _reloadProgress;

        [Header("Show Animations")]
        [SerializeField] private ScaleAnimation _aimScaleAnimation;
        [SerializeField] private FadeAnimation _aimFadeAnimation;
        [SerializeField] private FadeAnimation _reloadProgressFadeAnimation;

        private WeaponHolder _weaponHolder;

        [Inject]
        private void Constructor(WeaponHolder weaponHolder)
        {
            _weaponHolder = weaponHolder;
        }

        private IAnimation _showAnimation;

        private IDisposable _weaponSubscription;
        private IDisposable _isWeaponReloadingSubscription;

        private readonly BoolReactiveProperty _isActive = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;

        #region MonoBehaviour

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_aimScaleAnimation, _aimFadeAnimation, _reloadProgressFadeAnimation);

            _showAnimation.SetStartState();
            _aim.gameObject.SetActive(true);
            _reloadProgress.gameObject.SetActive(false);
            _isActive.Value = false;
        }

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        public void Show(Action onComplete = null)
        {
            _reloadProgress.SetActive(true);
            _isActive.Value = true;
            _showAnimation.PlayForward(() =>
            {
                _aim.SetActive(false);
                onComplete?.Invoke();
            });
        }

        public void Hide(Action onComplete = null)
        {
            _aim.SetActive(true);
            _showAnimation.PlayBackward(() =>
            {
                _reloadProgress.SetActive(false);
                _isActive.Value = false;
                onComplete?.Invoke();
            });
        }

        private void StartObserving() => StartObservingWeapon();

        private void StopObserving()
        {
            StopObservingWeapon();
            _isWeaponReloadingSubscription?.Dispose();
        }

        private void StartObservingWeapon() => _weaponSubscription = _weaponHolder.Instance.Subscribe(OnWeaponChanged);

        private void StopObservingWeapon() => _weaponSubscription?.Dispose();

        private void OnWeaponChanged(IWeapon weapon)
        {
            _isWeaponReloadingSubscription?.Dispose();

            if (weapon == null)
            {
                Hide();
                return;
            }

            _isWeaponReloadingSubscription = weapon.IsReloading.Subscribe(OnWeaponReloadingStateChanged);
        }

        private void OnWeaponReloadingStateChanged(bool isReloading)
        {
            if (isReloading)
                Show();
            else
                Hide();
        }
    }
}