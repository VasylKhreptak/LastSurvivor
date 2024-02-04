using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class Player : MonoBehaviour
    {
        [Inject]
        private void Constructor(PlayerWaypointNavigator waypointNavigator)
        {
            WaypointNavigator = waypointNavigator;
        }

        public PlayerWaypointNavigator WaypointNavigator { get; private set; }
    }
}