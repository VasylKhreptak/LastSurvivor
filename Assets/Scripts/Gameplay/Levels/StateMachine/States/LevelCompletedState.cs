using Gameplay.Data;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UI.Gameplay.Windows;

namespace Gameplay.Levels.StateMachine.States
{
    public class LevelCompletedState : ILevelState, IState
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly LevelData _levelData;
        private readonly LevelCompletedWindow _levelCompletedWindow;

        public LevelCompletedState(IStateMachine<ILevelState> levelStateMachine, LevelData levelData,
            LevelCompletedWindow levelCompletedWindow)
        {
            _levelStateMachine = levelStateMachine;
            _levelData = levelData;
            _levelCompletedWindow = levelCompletedWindow;
        }

        public void Enter()
        {
            _levelCompletedWindow.Show();
            _levelData.LevelResult = LevelResult.Completed;
            _levelStateMachine.Enter<PauseLevelState>();
        }
    }
}