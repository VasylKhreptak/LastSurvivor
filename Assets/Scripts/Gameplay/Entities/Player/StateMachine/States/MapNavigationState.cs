using Entities.AI;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Levels.StateMachine.States;
using Levels.StateMachine.States.Core;
using UnityEngine.AI;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class MapNavigationState : IPlayerState, IState, IExitable
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly IStateMachine<IPlayerState> _playerStateMachine;
        private readonly AgentWaypointsFollower _waypointsFollower;

        public MapNavigationState(IStateMachine<ILevelState> levelStateMachine, IStateMachine<IPlayerState> playerStateMachine,
            NavMeshAgent agent, Waypoints.Waypoints waypoints, AgentMover.Preferences movePreferences)
        {
            _levelStateMachine = levelStateMachine;
            _playerStateMachine = playerStateMachine;
            _waypointsFollower = new AgentWaypointsFollower(agent, waypoints, movePreferences);
        }

        public void Enter() =>
            _waypointsFollower.Start(() =>
            {
                _playerStateMachine.Enter<IdleState>();
                _levelStateMachine.Enter<LevelCompletedState>();
            });

        public void Exit() => _waypointsFollower.Stop();
    }
}