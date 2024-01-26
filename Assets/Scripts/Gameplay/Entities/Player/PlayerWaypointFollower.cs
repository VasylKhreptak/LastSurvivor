using Entities.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Gameplay.Waypoints;
using Infrastructure.StateMachine.Main.Core;

namespace Gameplay.Entities.Player
{
    public class PlayerWaypointFollower
    {
        private readonly IStateMachine<IPlayerState> _playerStateMachine;
        private readonly PlayerWaypoints _playerWaypoints;

        public PlayerWaypointFollower(IStateMachine<IPlayerState> playerStateMachine, PlayerWaypoints playerWaypoints)
        {
            _playerStateMachine = playerStateMachine;
            _playerWaypoints = playerWaypoints;
        }

        private AgentMoveState.Payload _moveStatePayload;

        public void Stop()
        {
            if (_moveStatePayload == null)
                return;

            _moveStatePayload.OnComplete = null;
            _moveStatePayload = null;
            _playerStateMachine.Enter<IdleState>();
        }

        public void Start()
        {
            Stop();
            TryMoveToNextWaypoint();
        }

        private void TryMoveToNextWaypoint()
        {
            Waypoint waypoint = _playerWaypoints.GetUnfinishedWaypoint();

            if (waypoint == null)
            {
                Stop();
                return;
            }

            AgentMoveState.Payload payload = new AgentMoveState.Payload
            {
                Position = waypoint.Position,
                OnComplete = () =>
                {
                    waypoint.MarkAsFinished();
                    TryMoveToNextWaypoint();
                }
            };

            _playerStateMachine.Enter<MoveState, AgentMoveState.Payload>(payload);
        }
    }
}