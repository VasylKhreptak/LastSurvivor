using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.StateMachine.Main;

namespace Gameplay.Entities.Soldier.StateMachine
{
    public class SoldierStateMachine : StateMachine<ISoldierState>
    {
        protected SoldierStateMachine(SoldierStateFactory stateFactory) : base(stateFactory) { }
    }
}