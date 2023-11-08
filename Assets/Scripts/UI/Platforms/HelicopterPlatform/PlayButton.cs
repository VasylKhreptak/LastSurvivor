using Data.Persistent;
using Infrastructure.Services.PersistentData.Core;
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
        private HelicopterData _helicopterData;

        [Inject]
        private void Constructor(IStateMachine<IGameState> stateMachine, IPersistentDataService persistentDataService)
        {
            _stateMachine = stateMachine;
            _helicopterData = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.HelicopterData;
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
            _stateMachine.Enter<LoadAppropriateLevelState>();
            _helicopterData.FuelTank.Clear();
        }
    }
}