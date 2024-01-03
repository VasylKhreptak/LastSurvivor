using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using Infrastructure.StateMachine.Main;

namespace Gameplay.Weapons.Minigun.StateMachine
{
    public class MinigunStateMachine : StateMachine<IMinigunState>
    {
        protected MinigunStateMachine(MinigunStateFactory stateFactory) : base(stateFactory) { }
    }
}