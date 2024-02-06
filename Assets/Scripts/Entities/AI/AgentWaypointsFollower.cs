using System;
using Gameplay.Waypoints;

namespace Entities.AI
{
    public class AgentWaypointsFollower
    {
        private readonly Waypoints _waypoints;
        private readonly AgentMover _agentMover;

        public AgentWaypointsFollower(Waypoints waypoints, AgentMover agentMover)
        {
            _waypoints = waypoints;
            _agentMover = agentMover;
        }

        public void Start(Action onCompleted = null)
        {
            Stop();

            Waypoint waypoint = _waypoints.GetNextWaypoint();

            if (waypoint == null)
            {
                onCompleted?.Invoke();
                return;
            }

            _agentMover.SetDestination(waypoint.Position, onComplete: () =>
            {
                waypoint.MarkAsFinished();
                Start(onCompleted);
            });
        }

        public void Stop() => _agentMover.Stop();
    }
}