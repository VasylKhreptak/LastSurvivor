using System;
using Infrastructure.Transition.Core;
using Plugins.Animations;
using UnityEngine;

namespace Infrastructure.Transition
{
    public class TransitionScreen : MonoBehaviour, ITransitionScreen
    {
        [Header("Preferences")]
        [SerializeField] private FadeAnimation _showAnimation;

        #region MonoBehaviour

        private void OnDestroy()
        {
            _showAnimation.Stop();
        }

        #endregion

        public void Show(Action onComplete = null)
        {
            gameObject.SetActive(true);
            _showAnimation.PlayForward(() => { onComplete?.Invoke(); });
        }

        public void Hide(Action onComplete = null)
        {
            _showAnimation.PlayBackward(() =>
            {
                onComplete?.Invoke();
                gameObject.SetActive(false);
            });
        }

        public void ShowImmediately()
        {
            gameObject.SetActive(true);
            _showAnimation.SetEndState();
        }

        public void HideImmediately()
        {
            _showAnimation.SetStartState();
            gameObject.SetActive(false);
        }
    }
}