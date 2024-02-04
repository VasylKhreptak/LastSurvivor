using Gameplay.Entities.Health.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class Player : MonoBehaviour
    {
        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health, PlayerWaypointNavigator waypointNavigator)
        {
            _health = health;
            WaypointNavigator = waypointNavigator;
        }

        public PlayerWaypointNavigator WaypointNavigator { get; private set; }
    }
}