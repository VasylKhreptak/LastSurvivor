using UnityEngine;

namespace Gameplay.Waypoints
{
    public class Waypoint
    {
        public readonly Vector3 Position;
        public readonly bool IsLast;

        public Waypoint(Vector3 position, bool isLast)
        {
            Position = position;
            IsLast = isLast;
        }

        public bool Finished { get; private set; }

        public void MarkAsFinished() => Finished = true;
    }
}