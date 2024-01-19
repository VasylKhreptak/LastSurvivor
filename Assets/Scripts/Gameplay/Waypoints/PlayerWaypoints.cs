using UnityEngine;

namespace Gameplay.Waypoints
{
    public class PlayerWaypoints
    {
        private readonly Waypoint[] _waypoints;

        public PlayerWaypoints(Transform[] transforms)
        {
            _waypoints = new Waypoint[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
            {
                _waypoints[i] = new Waypoint(transforms[i].position);
            }
        }

        public Waypoint GetUnfinishedWaypoint()
        {
            foreach (Waypoint waypoint in _waypoints)
            {
                if (!waypoint.Finished)
                    return waypoint;
            }

            return null;
        }
    }
}