using Gameplay.Data;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UI.Gameplay.Windows;

namespace Gameplay.Levels.StateMachine.States
{
    public class LevelFailedState : ILevelState, IState
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly LevelFailedWindow _levelFailedWindow;
        private readonly LevelData _levelData;

        public LevelFailedState(IStateMachine<ILevelState> levelStateMachine, LevelFailedWindow levelFailedWindow, LevelData levelData)
        {
            _levelStateMachine = levelStateMachine;
            _levelFailedWindow = levelFailedWindow;
            _levelData = levelData;
        }

        public void Enter()
        {
            _levelFailedWindow.Show();
            _levelData.LevelResult = LevelResult.Failed;
            _levelStateMachine.Enter<PauseLevelState>();
        }
    }
}