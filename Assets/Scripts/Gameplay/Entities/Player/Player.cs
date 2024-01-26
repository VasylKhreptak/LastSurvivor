using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class Player : MonoBehaviour
    {
        private PlayerWaypointFollower _waypointFollower;

        [Inject]
        private void Constructor(IStateMachine<IPlayerState> stateMachine, PlayerWaypointFollower waypointFollower)
        {
            _waypointFollower = waypointFollower;
        }
        
        public PlayerWaypointFollower WaypointFollower => _waypointFollower;
    }
}