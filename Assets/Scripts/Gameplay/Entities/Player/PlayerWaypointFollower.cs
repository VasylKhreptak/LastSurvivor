using System;
using Entities.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Gameplay.Waypoints;
using Infrastructure.StateMachine.Main.Core;
using Levels.StateMachine.States;
using Levels.StateMachine.States.Core;

namespace Gameplay.Entities.Player
{
    public class PlayerWaypointFollower : IDisposable
    {
        private readonly IStateMachine<IPlayerState> _playerStateMachine;
        private readonly PlayerWaypoints _playerWaypoints;
        private readonly IStateMachine<ILevelState> _levelStateMachine;

        public PlayerWaypointFollower(IStateMachine<IPlayerState> playerStateMachine, PlayerWaypoints playerWaypoints,
            IStateMachine<ILevelState> levelStateMachine)
        {
            _playerStateMachine = playerStateMachine;
            _playerWaypoints = playerWaypoints;
            _levelStateMachine = levelStateMachine;
        }

        private bool _finished;

        public void Dispose() => Stop();

        public void Start() => StartWaypointMovement();

        public void Stop() => StopWaypointMovement();

        private void StartWaypointMovement()
        {
            if (_finished)
                return;

            Waypoint waypoint = _playerWaypoints.GetNextWaypoint();

            if (waypoint == null)
            {
                _playerStateMachine.Enter<IdleState>();
                _levelStateMachine.Enter<LevelFinishedState>();
                _finished = true;
                return;
            }

            AgentMoveState.Payload payload = new AgentMoveState.Payload
            {
                Position = waypoint.Position,
                OnComplete = () =>
                {
                    waypoint.MarkAsFinished();
                    StartWaypointMovement();
                }
            };

            _playerStateMachine.Enter<MoveState, AgentMoveState.Payload>(payload);
        }

        private void StopWaypointMovement() => _playerStateMachine.Enter<IdleState>();
    }
}