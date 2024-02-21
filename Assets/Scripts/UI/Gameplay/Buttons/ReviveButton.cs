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

        [Header("Show Animations")]
        [SerializeField] private AnchorMoveAnimation _anchorMoveAnimation;
        [SerializeField] private FadeAnimation _fadeAnimation;

        [Header("Preferences")]
        [SerializeField] private float _levelResumeDelay = 0.5f;

        private IWindow _levelFailedWindow;
        private PlayerHolder _playerHolder;
        private IStateMachine<ILevelState> _levelStateMachine;

        [Inject]
        private void Constructor(LevelFailedWindow levelFailedWindow, PlayerHolder playerHolder,
            IStateMachine<ILevelState> levelStateMachine)
        {
            _levelFailedWindow = levelFailedWindow;
            _playerHolder = playerHolder;
            _levelStateMachine = levelStateMachine;
        }

        private IAnimation _showAnimation;

        private IDisposable _levelResumeDelaySubscription;

        #region MonoBehaviour

        private void OnValidate() => _button ??= GetComponentInChildren<Button>();

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_anchorMoveAnimation, _fadeAnimation);
            _showAnimation.SetEndState();
        }

        private void OnEnable() => StartObserving();

        private void OnDisable()
        {
            StopObserving();
        }

        private void OnDestroy() => _levelResumeDelaySubscription?.Dispose();

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

        private void StartObserving() => _button.onClick.AddListener(OnClicked);

        private void StopObserving() => _button.onClick.RemoveListener(OnClicked);

        private void OnClicked()
        {
            Hide(() => _levelFailedWindow.Hide(() =>
            {
                if (_playerHolder.Instance != null && _playerHolder.Instance.Health.IsDeath.Value)
                    _playerHolder.Instance.StateMachine.Enter<ReviveState>();

                _levelResumeDelaySubscription?.Dispose();
                _levelResumeDelaySubscription = Observable
                    .Timer(TimeSpan.FromSeconds(_levelResumeDelay))
                    .Subscribe(_ => _levelStateMachine.Enter<ResumeLevelState>());

                _continueButton.Show();
            }));

            _button.interactable = false;
        }
    }
}