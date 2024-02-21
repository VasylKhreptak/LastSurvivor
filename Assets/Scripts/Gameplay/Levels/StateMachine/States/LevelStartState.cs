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

        public LevelStartState(IStateMachine<ILevelState> levelStateMachine,
            StartWindow startWindow)
        {
            _levelStateMachine = levelStateMachine;
            _startWindows = startWindow;
        }

        public void Enter()
        {
            _levelStateMachine.Enter<ResumeLevelState>();
            _startWindows.Hide();
        }
    }
}