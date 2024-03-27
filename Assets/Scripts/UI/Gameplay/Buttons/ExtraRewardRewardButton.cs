using System;
using Gameplay.Data;
using Infrastructure.Services.Advertisement.Core;
using Infrastructure.Services.ToastMessage.Core;
using Plugins.Animations;
using Plugins.Animations.Core;
using Plugins.Animations.Move;
using Plugins.Timer;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Gameplay.Buttons
{
    public class ExtraRewardRewardButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Button _button;
        [SerializeField] private ContinueButton _continueButton;
        [SerializeField] private TMP_Text _leftSecondsText;

        [Header("Preferences")]
        [SerializeField] private float _showDuration = 4f;

        [Header("Show Animations")]
        [SerializeField] private AnchorMoveAnimation _anchorMoveAnimation;
        [SerializeField] private FadeAnimation _fadeAnimation;

        [Header("Reward Preferences")]
        [SerializeField] private float _rewardMultiplier = 1.1f;

        private LevelData _levelData;
        private IAdvertisementService _advertisementService;

        [Inject]
        private void Constructor(LevelData levelData, IAdvertisementService advertisementService)
        {
            _levelData = levelData;
            _advertisementService = advertisementService;
        }

        private IToastMessageService _toastMessageService;

        [Inject]
        private void Constructor(IToastMessageService toastMessageService)
        {
            _toastMessageService = toastMessageService;
        }

        private IAnimation _showAnimation;

        private IDisposable _timerSubscription;
        private IDisposable _leftSecondsSubscription;

        private readonly ITimer _hideTimer = new Timer();

        #region MonoBehaviour

        private void OnValidate() => _button ??= GetComponentInChildren<Button>();

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_anchorMoveAnimation, _fadeAnimation);
            _showAnimation.SetEndState();
        }

        private void OnEnable()
        {
            StartObservingButton();
            StartObservingTimerCompletion();
            StartObservingLeftSeconds();
            StartHideTimer();
        }

        private void OnDisable()
        {
            StopObservingButton();
            StopObservingTimerCompletion();
            StopObservingLeftSeconds();
            _hideTimer.Stop();
        }

        #endregion

        private void StartObservingButton() => _button.onClick.AddListener(OnClicked);

        private void StopObservingButton() => _button.onClick.RemoveListener(OnClicked);

        private void StartObservingTimerCompletion() => _timerSubscription = _hideTimer.OnCompleted.Subscribe(_ => OnTimerCompleted());

        private void StopObservingTimerCompletion() => _timerSubscription?.Dispose();

        private void StartObservingLeftSeconds()
        {
            _leftSecondsSubscription = _hideTimer.RemainingTime
                .Select(Mathf.CeilToInt)
                .DistinctUntilChanged()
                .Subscribe(OnRemainingSecondsChanged);
        }

        private void StopObservingLeftSeconds() => _leftSecondsSubscription?.Dispose();

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

        private void OnClicked()
        {
            _hideTimer.Stop();

            if (_advertisementService.ShowRewardedVideo(OnRewarded) == false)
            {
                Hide();
                _continueButton.Show();
                _toastMessageService.Send("No video available");
            }

            _button.interactable = false;
        }

        private void StartHideTimer() => _hideTimer.Start(_showDuration);

        private void OnTimerCompleted()
        {
            Hide();
            _button.interactable = false;
            _continueButton.Show();
        }

        private void OnRemainingSecondsChanged(int remainingSeconds) => _leftSecondsText.text = remainingSeconds.ToString();

        private void OnRewarded()
        {
            AddExtraReward();
            Hide();
            _continueButton.Show();
        }

        private void AddExtraReward()
        {
            _levelData.CollectedGears.Value = (int)(_levelData.CollectedGears.Value * _rewardMultiplier);
            _levelData.CollectedMoney.Value = (int)(_levelData.CollectedMoney.Value * _rewardMultiplier);
        }
    }
}