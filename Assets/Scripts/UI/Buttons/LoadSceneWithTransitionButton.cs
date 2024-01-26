using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public class LoadSceneWithTransitionButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        [Header("Preferences")]
        [SerializeField] private SceneField _scene;

        private IStateMachine<IGameState> _stateMachine;

        [Inject]
        private void Constructor(IStateMachine<IGameState> stateMachine) => _stateMachine = stateMachine;

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
            LoadScene();
        }

        private void LoadScene()
        {
            LoadSceneAsyncState.Payload payload = new LoadSceneAsyncState.Payload
            {
                Name = _scene.Name
            };

            _stateMachine.Enter<LoadSceneWithTransitionAsyncState, LoadSceneAsyncState.Payload>(payload);
        }
    }
}