using System;
using Animations;
using Data.Persistent;
using Infrastructure.Services.PersistentData.Core;
using Plugins.Animations;
using Plugins.Animations.Core;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Platforms.HelicopterPlatform
{
    public class HelicopterInfoStateAnimation : MonoBehaviour
    {
        [Header("Animations")]
        [SerializeField] private Button _playButton;
        [SerializeField] private ScalePressAnimation _playButtonPressAnimation;
        [SerializeField] private SizeDeltaAnimation _panelExpandAnimation;
        [SerializeField] private FadeAnimation _buttonAppearAnimation;
        [SerializeField] private ScaleAnimation _buttonScaleUpAnimation;

        private HelicopterData _helicopterData;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _helicopterData = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.HelicopterData;
        }

        private IAnimation _expandAnimation;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void Awake()
        {
            _expandAnimation = new AnimationGroup(_panelExpandAnimation, _buttonAppearAnimation, _buttonScaleUpAnimation);
        }

        private void OnEnable()
        {
            UpdateStateImmediately();
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _expandAnimation.Stop();
        }

        #endregion

        private void StartObserving() => _subscription = _helicopterData.FuelTank.IsFilled.Subscribe(_ => UpdateState());

        private void StopObserving() => _subscription?.Dispose();

        private void UpdateState()
        {
            if (_helicopterData.FuelTank.IsFilled.Value)
                Expand();
            else
                Shrink();
        }

        [Button]
        private void Expand()
        {
            _playButton.interactable = true;
            _playButtonPressAnimation.Stop();
            _expandAnimation.PlayForward();
        }

        [Button]
        private void Shrink()
        {
            _playButton.interactable = false;
            _playButtonPressAnimation.Stop();
            _expandAnimation.PlayBackward();
        }

        private void UpdateStateImmediately()
        {
            if (_helicopterData.FuelTank.IsFilled.Value)
                ExpandImmediately();
            else
                ShrinkImmediately();
        }

        [Button]
        private void ExpandImmediately()
        {
            _playButtonPressAnimation.Stop();
            _expandAnimation.SetEndState();
            _playButton.interactable = true;
        }

        [Button]
        private void ShrinkImmediately()
        {
            _playButtonPressAnimation.Stop();
            _expandAnimation.SetStartState();
            _playButton.interactable = false;
        }
    }
}