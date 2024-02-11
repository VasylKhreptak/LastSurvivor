using UnityEngine;

namespace Gameplay.Waypoints
{
    public class Waypoints
    {
        private readonly Waypoint[] _waypoints;

        public Waypoints(Transform[] transforms)
        {
            _waypoints = new Waypoint[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
            {
                _waypoints[i] = new Waypoint(transforms[i].position, i == transforms.Length - 1);
            }
        }

        public Waypoint GetNextWaypoint()
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