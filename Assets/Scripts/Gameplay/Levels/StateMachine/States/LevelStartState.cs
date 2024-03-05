using Gameplay.Levels.Analytics;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UI.Gameplay.Windows;

namespace Gameplay.Levels.StateMachine.States
{
    public class LevelStartState : ILevelState, IState
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly StartWindow _startWindows;
        private readonly LevelEventLogger _levelEventLogger;

        public LevelStartState(IStateMachine<ILevelState> levelStateMachine, StartWindow startWindow,
            LevelEventLogger levelEventLogger)
        {
            _levelStateMachine = levelStateMachine;
            _startWindows = startWindow;
            _levelEventLogger = levelEventLogger;
        }

        public void Enter()
        {
            _startWindows.Hide();
            _levelEventLogger.LogLevelStartedEvent();
            _levelStateMachine.Enter<ResumeLevelState>();
        }
    }
}