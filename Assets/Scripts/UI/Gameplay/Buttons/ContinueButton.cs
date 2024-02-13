using Infrastructure.StateMachine.Main.Core;
using Levels.StateMachine.States;
using Levels.StateMachine.States.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Gameplay.Buttons
{
    public class ContinueButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        private IStateMachine<ILevelState> _stateMachine;

        [Inject]
        private void Constructor(IStateMachine<ILevelState> stateMachine) => _stateMachine = stateMachine;

        #region MonoBehaviour

        private void OnValidate() => _button ??= GetComponent<Button>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _button.onClick.AddListener(OnClicked);

        private void StopObserving() => _button.onClick.RemoveListener(OnClicked);

        private void OnClicked()
        {
            _button.interactable = false;
            _stateMachine.Enter<FinalizeProgressAndLoadMenuState>();
        }
    }
}