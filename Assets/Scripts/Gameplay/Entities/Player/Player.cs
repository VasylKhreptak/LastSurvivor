using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerWaypointNavigator WaypointNavigator { get; private set; }

        [Inject]
        private void Constructor(PlayerWaypointNavigator waypointNavigator)
        {
            WaypointNavigator = waypointNavigator;
        }
    }
}