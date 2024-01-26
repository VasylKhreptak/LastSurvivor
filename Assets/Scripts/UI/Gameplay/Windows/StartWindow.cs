using System;
using Infrastructure.Graphics.UI.Windows.Core;
using Plugins.Animations;
using Plugins.Animations.Core;
using Plugins.Animations.Move;
using UniRx;
using UnityEngine;

namespace UI.Gameplay.Windows
{
    public class StartWindow : MonoBehaviour, IWindow
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        [Header("Show Animations")]
        [SerializeField] private AnchorMoveAnimation _sceneNameAnimation;
        [SerializeField] private AnchorMoveAnimation _tapToPlayTextAnimation;
        [SerializeField] private FadeAnimation _canvasFadeAnimation;

        private IAnimation _showAnimation;

        private readonly BoolReactiveProperty _isActive = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;

        #region MonoBehaviour

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_sceneNameAnimation, _tapToPlayTextAnimation, _canvasFadeAnimation);
            _showAnimation.SetEndState();
            _gameObject.SetActive(true);
            _isActive.Value = true;
        }

        #endregion

        public void Show(Action onComplete = null)
        {
            _gameObject.SetActive(true);
            _showAnimation.PlayForward(() => onComplete?.Invoke());
            _isActive.Value = true;
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