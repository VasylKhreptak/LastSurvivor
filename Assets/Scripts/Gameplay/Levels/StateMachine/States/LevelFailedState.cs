using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UI.Gameplay.Windows;

namespace Gameplay.Levels.StateMachine.States
{
    public class LevelFailedState : ILevelState, IState
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly LevelFailedWindow _levelFailedWindow;
        private readonly IPersistentDataService _persistentDataService;

        public LevelFailedState(IStateMachine<ILevelState> levelStateMachine, LevelFailedWindow levelFailedWindow,
            IPersistentDataService persistentDataService)
        {
            _levelStateMachine = levelStateMachine;
            _levelFailedWindow = levelFailedWindow;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            _levelStateMachine.Enter<PauseLevelState>();
            _levelFailedWindow.Show();
            _persistentDataService.Data.PlayerData.PlatformsData.CollectorsPlatformData.CollectorsBank.Clear();
            _persistentDataService.Data.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.Clear();
        }
    }
}