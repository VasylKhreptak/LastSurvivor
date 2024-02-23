using System;
using Gameplay.Entities.Player;
using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Levels.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.Graphics.UI.Windows.Core;
using Infrastructure.StateMachine.Main.Core;
using Plugins.Animations;
using Plugins.Animations.Core;
using Plugins.Animations.Move;
using Plugins.Timer;
using TMPro;
using UI.Gameplay.Windows;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Gameplay.Buttons
{
    public class ReviveButton : MonoBehaviour, IWindow
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Button _button;
        [SerializeField] private ContinueButton _continueButton;
        [SerializeField] private TMP_Text _leftSecondsText;

        [Header("Preferences")]
        [SerializeField] private float _levelResumeDelay = 0.5f;
        [SerializeField] private float _showDuration = 3f;

        [Header("Show Animations")]
        [SerializeField] private AnchorMoveAnimation _anchorMoveAnimation;
        [SerializeField] private FadeAnimation _fadeAnimation;

        private IWindow _levelFailedWindow;
        private Player _player;
        private IStateMachine<ILevelState> _levelStateMachine;

        [Inject]
        private void Constructor(LevelFailedWindow levelFailedWindow, Player player, IStateMachine<ILevelState> levelStateMachine)
        {
            _levelFailedWindow = levelFailedWindow;
            _player = player;
            _levelStateMachine = levelStateMachine;
        }

        private IAnimation _showAnimation;

        private IDisposable _levelResumeDelaySubscription;
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

        private void OnDestroy() => _levelResumeDelaySubscription?.Dispose();

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
            _levelFailedWindow.Hide(() =>
            {
                if (_player.Health.IsDeath.Value)
                    _player.StateMachine.Enter<ReviveState>();

                _levelResumeDelaySubscription?.Dispose();
                _levelResumeDelaySubscription = Observable
                    .Timer(TimeSpan.FromSeconds(_levelResumeDelay))
                    .Subscribe(_ => _levelStateMachine.Enter<ResumeLevelState>());

                _continueButton.Show();
                Hide();
            });

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
    }
}