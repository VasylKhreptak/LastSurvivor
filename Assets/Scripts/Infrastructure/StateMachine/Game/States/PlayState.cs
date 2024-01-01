using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.Transition.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class PlayState : IState, IGameState, IExitable
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ITransitionScreen _transitionScreen;
        private readonly IPersistentDataService _persistentDataService;

        public PlayState(IStateMachine<IGameState> stateMachine, ITransitionScreen transitionScreen,
            IPersistentDataService persistentDataService)
        {
            _stateMachine = stateMachine;
            _transitionScreen = transitionScreen;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            StartObservingTransitionScreen();
            _transitionScreen.Show();
        }

        public void Exit()
        {
            StopObservingTransitionScreen();
            _transitionScreen.Hide();
        }

        private void StartObservingTransitionScreen() => _transitionScreen.OnShown += OnTransitionScreenShown;

        private void StopObservingTransitionScreen() => _transitionScreen.OnShown -= OnTransitionScreenShown;

        private void OnTransitionScreenShown()
        {
            _persistentDataService.PersistentData.PlayerData.PlatformsData.HelicopterPlatformData.FuelTank.Clear();
            _stateMachine.Enter<LoadAppropriateLevelState>();
        }
    }
}