using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Gameplay.Waypoints;
using Infrastructure.StateMachine.Main.Core;
using Zenject.Infrastructure.Toggleable.Core;

namespace Gameplay.Entities.Player
{
    public class PlayerWaypointFollower : IEnableable, IDisableable
    {
        private readonly IStateMachine<IPlayerState> _stateMachine;
        private readonly PlayerWaypoints _playerWaypoints;

        public PlayerWaypointFollower(IStateMachine<IPlayerState> stateMachine, PlayerWaypoints playerWaypoints)
        {
            _stateMachine = stateMachine;
            _playerWaypoints = playerWaypoints;
        }

        private MoveState.Payload _moveStatePayload;

        public void Enable() => MoveToNextWaypoint();

        public void Disable()
        {
            if (_moveStatePayload == null)
                return;

            _moveStatePayload.OnComplete = null;
            _stateMachine.Enter<IdleState>();
        }

        private void MoveToNextWaypoint()
        {
            Waypoint waypoint = _playerWaypoints.GetUnfinishedWaypoint();

            if (waypoint == null)
                return;

            MoveState.Payload payload = new MoveState.Payload
            {
                Position = waypoint.Position,
                OnComplete = () =>
                {
                    waypoint.MarkAsFinished();
                    MoveToNextWaypoint();
                }
            };

            _stateMachine.Enter<MoveState, MoveState.Payload>(payload);
        }
    }
}