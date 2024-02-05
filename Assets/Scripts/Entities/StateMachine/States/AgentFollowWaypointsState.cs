using System;
using Entities.AI;
using Gameplay.Waypoints;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine.AI;

namespace Entities.StateMachine.States
{
    public class AgentFollowWaypointsState : IPayloadedState<Action>, IExitable
    {
        private readonly AgentWaypointsFollower _waypointsFollower;

        public AgentFollowWaypointsState(NavMeshAgent agent, Waypoints waypoints, AgentMover.Preferences preferences)
        {
            _waypointsFollower = new AgentWaypointsFollower(agent, waypoints, preferences);
        }

        public void Enter(Action onComplete) => _waypointsFollower.Start(onComplete);

        public void Exit() => _waypointsFollower.Stop();
    }
}