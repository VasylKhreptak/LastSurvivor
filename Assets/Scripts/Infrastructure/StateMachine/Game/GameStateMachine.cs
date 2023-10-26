using Infrastructure.StateMachine.Game.Factory;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main;
using Zenject;

namespace Infrastructure.StateMachine.Game
{
    public class GameStateMachine : StateMachine<IGameState>, ITickable
    {
        protected GameStateMachine(GameStateFactory stateFactory) : base(stateFactory) { }

        public void Tick()
        {
            Update();
        }
    }
}