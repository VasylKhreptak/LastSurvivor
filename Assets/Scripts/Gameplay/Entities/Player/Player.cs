using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class Player : MonoBehaviour
    {
        [Inject]
        private void Constructor(IStateMachine<IPlayerState> stateMachine, PlayerWaypointFollower waypointFollower)
        {
            WaypointFollower = waypointFollower;
        }

        public PlayerWaypointFollower WaypointFollower { get; private set; }
    }
}