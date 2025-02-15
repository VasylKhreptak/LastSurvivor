﻿using System;
using Gameplay.Levels.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.Graphics.UI.Windows.Core;
using Infrastructure.StateMachine.Main.Core;
using Plugins.Animations;
using Plugins.Animations.Core;
using Plugins.Animations.Move;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Gameplay.Buttons
{
    public class ContinueButton : MonoBehaviour, IWindow
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Button _button;

        [Header("Show Animations")]
        [SerializeField] private AnchorMoveAnimation _anchorMoveAnimation;
        [SerializeField] private FadeAnimation _fadeAnimation;

        private IStateMachine<ILevelState> _stateMachine;

        [Inject]
        private void Constructor(IStateMachine<ILevelState> stateMachine) => _stateMachine = stateMachine;

        private IAnimation _showAnimation;

        #region MonoBehaviour

        private void OnValidate() => _button ??= GetComponentInChildren<Button>();

        private void Awake()
        {
            _showAnimation = new AnimationGroup(_anchorMoveAnimation, _fadeAnimation);
            _showAnimation.SetStartState();
            _gameObject.SetActive(false);
        }

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

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
            _button.interactable = false;
            _stateMachine.Enter<FinalizeProgressAndLoadMenuState>();
        }
    }
}