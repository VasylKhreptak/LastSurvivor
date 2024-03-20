using Audio;
using Infrastructure.LoadingScreen.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Plugins.AudioService.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class SetupBackgroundMusicState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;
        private readonly BackgroundMusicPlayer _backgroundMusicPlayer;
        private readonly ILoadingScreen _loadingScreen;

        public SetupBackgroundMusicState(IStateMachine<IGameState> stateMachine, IAudioService audioService,
            IStaticDataService staticDataService, ILogService logService, ILoadingScreen loadingScreen)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _loadingScreen = loadingScreen;
            _backgroundMusicPlayer = new BackgroundMusicPlayer(audioService, staticDataService.Config.BackgroundMusicPreferences);
        }

        public void Enter()
        {
            _logService.Log("Started playing background music");
            _loadingScreen.SetInfoText("Setting up background music...");

            if (_backgroundMusicPlayer.IsPlaying() == false)
                _backgroundMusicPlayer.Play();

            EnterNextState();
        }

        private void EnterNextState() => _stateMachine.Enter<LoadMainSceneState>();
    }
}