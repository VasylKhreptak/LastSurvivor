using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.StateMachine.Main;

namespace Gameplay.Levels.StateMachine
{
    public class LevelStateMachine : StateMachine<ILevelState>
    {
        protected LevelStateMachine(LevelStateFactory stateFactory) : base(stateFactory) { }
    }
}