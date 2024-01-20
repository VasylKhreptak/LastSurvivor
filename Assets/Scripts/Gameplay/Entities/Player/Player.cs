using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class Player : MonoBehaviour
    {
        [Inject]
        private void Constructor(IStateMachine<IPlayerState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public IStateMachine<IPlayerState> StateMachine { get; private set; }
    }
}