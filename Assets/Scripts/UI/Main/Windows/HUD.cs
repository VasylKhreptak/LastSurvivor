using System;
using Infrastructure.Graphics.UI.Windows.Core;
using Plugins.Animations;
using Plugins.Animations.Core;
using Plugins.Animations.Move;
using UnityEngine;

namespace UI.Main.Windows
{
    public class HUD : MonoBehaviour, IWindow
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        [Header("Show Animations")]
        [SerializeField] private AnchorMoveAnimation _anchorMoveAnimation;
        [SerializeField] private FadeAnimation _fadeAnimation;

        private IAnimation _showAnimation;

        #region MonoBehaviour

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_anchorMoveAnimation, _fadeAnimation);
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