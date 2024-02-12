using System;
using Extensions;
using Gameplay.Waypoints;
using Pathfinding;
using UniRx;
using UnityEngine;

namespace Entities.AI
{
    public class AgentWaypointsFollower
    {
        private readonly IAstarAI _ai;
        private readonly Waypoints _waypoints;
        private readonly Preferences _preferences;

        public AgentWaypointsFollower(IAstarAI ai, Waypoints waypoints, Preferences preferences)
        {
            _ai = ai;
            _waypoints = waypoints;
            _preferences = preferences;
        }

        private IDisposable _updateSubscription;

        public void Start(Action onCompleted = null)
        {
            Stop();

            Waypoint waypoint = _waypoints.GetNextWaypoint();

            if (waypoint == null)
            {
                onCompleted?.Invoke();
                return;
            }

            _ai.isStopped = false;
            _ai.destination = waypoint.Position;
            _updateSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.UpdateInterval))
                .Subscribe(_ =>
                {
                    bool canMoveToNextWaypoint = _ai.reachedEndOfPath ||
                                                 (waypoint.IsLast == false &&
                                                  _ai.position.IsCloseTo(_ai.destination, _preferences.WaypointThreshold)) ||
                                                 (waypoint.IsLast && _ai.reachedEndOfPath);

                    if (canMoveToNextWaypoint)
                    {
                        waypoint.MarkAsFinished();
                        Start(onCompleted);
                    }
                });
        }

        public void Stop()
        {
            _ai.isStopped = true;
            _updateSubscription?.Dispose();
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _waypointThreshold = 1f;
            [SerializeField] private float _updateInterval = 0.1f;

            public float WaypointThreshold => _waypointThreshold;
            public float UpdateInterval => _updateInterval;
        }
    }
}