using System;
using Infrastructure.Graphics.UI.Windows.Core;
using Plugins.Animations;
using Plugins.Animations.Core;
using UnityEngine;

namespace UI.Gameplay.Windows
{
    public class LevelCompletedWindow : MonoBehaviour, IWindow
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        [Header("Show Animations")]
        [SerializeField] private ScaleAnimation _scaleAnimation;
        [SerializeField] private FadeAnimation _canvasFadeAnimation;

        private IAnimation _showAnimation;

        #region MonoBehaviour

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_scaleAnimation, _canvasFadeAnimation);
            _showAnimation.SetStartState();
            _gameObject.SetActive(false);
        }

        #endregion

        public void Show(Action onComplete = null)
        {
            _gameObject.SetActive(true);
            _showAnimation.PlayForward(() => onComplete?.Invoke());
        }

        public void Hide(Action onComplete = null)
        {
            _showAnimation.PlayBackward(() =>
            {
                _gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
    }
}