using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Platforms.HelicopterPlatform
{
    public class PlayButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        private IStateMachine<IGameState> _stateMachine;

        [Inject]
        private void Constructor(IStateMachine<IGameState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        #region MonoBehaivour

        private void OnValidate() => _button ??= GetComponent<Button>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _button.onClick.AddListener(OnClicked);

        private void StopObserving() => _button.onClick.RemoveListener(OnClicked);

        private void OnClicked()
        {
            StopObserving();

            _stateMachine.Enter<PlayState>();
        }
    }
}