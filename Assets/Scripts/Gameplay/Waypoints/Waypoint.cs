using UnityEngine;

namespace Gameplay.Waypoints
{
    public class Waypoint
    {
        public readonly Vector3 Position;

        public Waypoint(Vector3 position)
        {
            Position = position;
        }

        public bool Finished { get; private set; }

        public void MarkAsFinished() => Finished = true;
    }
}