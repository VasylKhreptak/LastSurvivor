using System;

namespace Gameplay.Entities.Player
{
    public class PlayerMapNavigator : IDisposable
    {
        private readonly PlayerWaypointNavigator _waypointNavigator;

        public PlayerMapNavigator(PlayerWaypointNavigator waypointNavigator)
        {
            _waypointNavigator = waypointNavigator;
        }

        public void Dispose() => Stop();

        public void Start() => _waypointNavigator.Start();

        public void Stop() => _waypointNavigator.Stop();
    }
}