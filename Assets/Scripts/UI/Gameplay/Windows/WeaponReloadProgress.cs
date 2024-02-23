using System;
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

        private IWeapon _weapon;

        [Inject]
        private void Constructor(IWeapon weapon)
        {
            _weapon = weapon;
        }

        private IAnimation _showAnimation;

        private IDisposable _weaponSubscription;
        private IDisposable _isWeaponReloadingSubscription;

        #region MonoBehaviour

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_aimScaleAnimation, _aimFadeAnimation, _reloadProgressFadeAnimation);

            _showAnimation.SetStartState();
            _aim.gameObject.SetActive(true);
            _reloadProgress.gameObject.SetActive(false);
        }

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            _isWeaponReloadingSubscription = _weapon.IsReloading.Subscribe(OnWeaponReloadingStateChanged);
        }

        private void StopObserving() => _isWeaponReloadingSubscription?.Dispose();
        
        private void OnWeaponReloadingStateChanged(bool isReloading)
        {
            if (isReloading)
                Show();
            else
                Hide();
        }
        
        public void Show(Action onComplete = null)
        {
            _reloadProgress.SetActive(true);
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
                onComplete?.Invoke();
            });
        }
    }
}