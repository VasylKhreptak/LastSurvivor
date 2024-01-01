using Infrastructure.Curtain.Core;
using Infrastructure.SceneManagement.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingScreen _loadingScreen;

        public LoadLevelState(IStateMachine<IGameState> gameStateMachine,
            ISceneLoader sceneLoader,
            ILoadingScreen loadingScreen)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
        }

        public void Enter(string payload) => _sceneLoader.LoadAsync(payload, OnLoadedScene);

        private void OnLoadedScene()
        {
            _loadingScreen.Hide();
            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}