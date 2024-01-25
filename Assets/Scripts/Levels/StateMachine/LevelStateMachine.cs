using Infrastructure.StateMachine.Main;
using Levels.StateMachine.States.Core;

namespace Levels.StateMachine
{
    public class LevelStateMachine : StateMachine<ILevelState>
    {
        protected LevelStateMachine(LevelStateFactory stateFactory) : base(stateFactory) { }
    }
}