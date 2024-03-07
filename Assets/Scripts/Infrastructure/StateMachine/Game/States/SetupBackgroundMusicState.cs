using Audio;
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

        public SetupBackgroundMusicState(IStateMachine<IGameState> stateMachine, IAudioService audioService,
            IStaticDataService staticDataService, ILogService logService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _backgroundMusicPlayer = new BackgroundMusicPlayer(audioService, staticDataService.Config.BackgroundMusicPreferences);
        }

        public void Enter()
        {
            _logService.Log("Started playing background music");
            _backgroundMusicPlayer.Play();
            _stateMachine.Enter<GameLoopState>();
        }
    }
}