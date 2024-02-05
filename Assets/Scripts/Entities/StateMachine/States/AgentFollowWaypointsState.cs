using System;
using Entities.AI;
using Infrastructure.StateMachine.Main.States.Core;

namespace Entities.StateMachine.States
{
    public class AgentFollowWaypointsState : IPayloadedState<Action>, IExitable
    {
        private readonly AgentWaypointsFollower _waypointsFollower;

        public AgentFollowWaypointsState(AgentWaypointsFollower waypointsFollower)
        {
            _waypointsFollower = waypointsFollower;
        }

        public void Enter(Action onComplete) => _waypointsFollower.Start(onComplete);

        public void Exit() => _waypointsFollower.Stop();
    }
}