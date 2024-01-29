using System;
using Infrastructure.Graphics.UI.Windows.Core;
using Plugins.Animations;
using Plugins.Animations.Core;
using UniRx;
using UnityEngine;

namespace UI.Gameplay.Windows
{
    public class WeaponAim : MonoBehaviour, IWindow
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        [Header("Show Animations")]
        [SerializeField] private ScaleAnimation _scaleAnimation;
        [SerializeField] private FadeAnimation _fadeAnimation;

        private readonly ReactiveProperty<bool> _isActive = new ReactiveProperty<bool>(false);

        private IAnimation _showAnimation;

        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;

        #region MonoBehaviour

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_scaleAnimation, _fadeAnimation);
            _showAnimation.SetStartState();
            _gameObject.SetActive(false);
            _isActive.Value = false;
        }

        #endregion

        public void Show(Action onComplete = null)
        {
            _gameObject.SetActive(true);
            _isActive.Value = true;
            _showAnimation.PlayForward(() => onComplete?.Invoke());
        }

        public void Hide(Action onComplete = null)
        {
            _showAnimation.PlayBackward(() =>
            {
                _gameObject.SetActive(false);
                _isActive.Value = false;
                onComplete?.Invoke();
            });
        }
    }
}