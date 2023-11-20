using System;
using Plugins.Animations;
using Plugins.Animations.Core;
using Plugins.Banks;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ClampedBanks
{
    public class ClampedBankMaxSign : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        [Header("Animations")]
        [SerializeField] private FadeAnimation _fadeShowAnimation;
        [SerializeField] private ScaleAnimation _scaleShowAnimation;

        private ClampedIntegerBank _clampedBank;

        [Inject]
        private void Constructor(ClampedIntegerBank clampedBank)
        {
            _clampedBank = clampedBank;
        }

        private IAnimation _showAnimation;

        private IDisposable _isFullSubscription;

        #region MonoBehaviour

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_fadeShowAnimation, _scaleShowAnimation);

            UpdateStateImmediately();
        }

        private void OnValidate()
        {
            if (transform.childCount > 0)
                _gameObject = transform.GetChild(0).gameObject;
        }

        private void OnEnable() => StartObserving();

        private void OnDisable()
        {
            StopObserving();
            _showAnimation.Stop();
        }

        #endregion

        private void StartObserving()
        {
            _isFullSubscription = _clampedBank.IsFull.Subscribe(isFull =>
            {
                if (isFull)
                    Show();
                else
                    Hide();
            });
        }

        private void StopObserving() => _isFullSubscription?.Dispose();

        private void Show()
        {
            _gameObject.SetActive(true);
            _showAnimation.PlayForward();
        }

        private void Hide() => _showAnimation.PlayBackward(() => _gameObject.SetActive(false));

        private void UpdateStateImmediately()
        {
            if (_clampedBank.IsFull.Value)
                _showAnimation.SetEndState();
            else
                _showAnimation.SetStartState();

            _gameObject.SetActive(_clampedBank.IsFull.Value);
        }
    }
}