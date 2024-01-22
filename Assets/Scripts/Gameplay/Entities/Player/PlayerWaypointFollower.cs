using Entities.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Waypoints;

namespace Gameplay.Entities.Player
{
    public class PlayerWaypointFollower
    {
        private readonly PlayerHolder _playerHolder;
        private readonly PlayerWaypoints _playerWaypoints;

        public PlayerWaypointFollower(PlayerHolder playerHolder, PlayerWaypoints playerWaypoints)
        {
            _playerHolder = playerHolder;
            _playerWaypoints = playerWaypoints;
        }

        private AgentMoveState.Payload _moveStatePayload;

        public void Stop()
        {
            if (_moveStatePayload == null)
                return;

            _moveStatePayload.OnComplete = null;
            _moveStatePayload = null;

            if (_playerHolder.Instance == null)
                return;

            _playerHolder.Instance.StateMachine.Enter<IdleState>();
        }

        public void Start()
        {
            Stop();
            TryMoveToNextWaypoint();
        }

        private void TryMoveToNextWaypoint()
        {
            if (_playerHolder.Instance == null)
            {
                Stop();
                return;
            }

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

            _playerHolder.Instance.StateMachine.Enter<MoveState, AgentMoveState.Payload>(payload);
        }
    }
}