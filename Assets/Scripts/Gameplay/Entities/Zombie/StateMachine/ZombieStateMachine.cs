using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main;

namespace Gameplay.Entities.Zombie.StateMachine
{
    public class ZombieStateMachine : StateMachine<IZombieState>
    {
        protected ZombieStateMachine(ZombieStateFactory stateFactory) : base(stateFactory) { }
    }
}