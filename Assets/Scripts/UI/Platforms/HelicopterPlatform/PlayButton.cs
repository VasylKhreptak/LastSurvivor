using Data.Persistent.Platforms;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.Transition.Core;
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
        private HelicopterPlatformData _helicopterPlatformData;
        private ITransitionScreen _transitionScreen;

        [Inject]
        private void Constructor(IStateMachine<IGameState> stateMachine, HelicopterPlatformData platformData,
            ITransitionScreen transitionScreen)
        {
            _stateMachine = stateMachine;
            _helicopterPlatformData = platformData;
            _transitionScreen = transitionScreen;
        }

        #region MonoBehaivour

        private void OnValidate() => _button ??= GetComponent<Button>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            _button.onClick.AddListener(OnClicked);
            _transitionScreen.OnShown += OnTransitionScreenShown;
        }

        private void StopObserving()
        {
            _button.onClick.RemoveListener(OnClicked);
            _transitionScreen.OnShown -= OnTransitionScreenShown;
        }

        private void OnClicked()
        {
            _button.onClick.RemoveListener(OnClicked);

            _transitionScreen.Show();
        }

        private void OnTransitionScreenShown()
        {
            _stateMachine.Enter<LoadAppropriateLevelState>();
            _helicopterPlatformData.FuelTank.Clear();
        }
    }
}