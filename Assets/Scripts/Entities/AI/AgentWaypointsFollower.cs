using System;
using Gameplay.Waypoints;
using UnityEngine.AI;

namespace Entities.AI
{
    public class AgentWaypointsFollower
    {
        private readonly Waypoints _waypoints;
        private readonly AgentMover _agentMover;

        public AgentWaypointsFollower(NavMeshAgent agent, Waypoints waypoints, AgentMover.Preferences preferences)
        {
            _waypoints = waypoints;
            _agentMover = new AgentMover(agent, preferences);
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

            _agentMover.SetDestination(waypoint.Position, () =>
            {
                waypoint.MarkAsFinished();
                Start(onCompleted);
            });
        }

        public void Stop() => _agentMover.Stop();
    }
}