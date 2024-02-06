using Entities.AI;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Levels.StateMachine.States.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class MapNavigationState : IPlayerState, IState, IExitable
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly IStateMachine<IPlayerState> _playerStateMachine;
        private readonly AgentWaypointsFollower _waypointsFollower;

        public MapNavigationState(IStateMachine<ILevelState> levelStateMachine, IStateMachine<IPlayerState> playerStateMachine,
            AgentWaypointsFollower waypointsFollower)
        {
            _levelStateMachine = levelStateMachine;
            _playerStateMachine = playerStateMachine;
            _waypointsFollower = waypointsFollower;
        }

        [Inject] private MeleeAttacker _meleeAttacker;

        public void Enter()
        {
            // _waypointsFollower.Start(() =>
            // {
            //     _playerStateMachine.Enter<IdleState>();
            //     _levelStateMachine.Enter<LevelCompletedState>();
            // });

            LootBox.LootBox lootBox = Object.FindObjectOfType<LootBox.LootBox>();
            _meleeAttacker.Start(lootBox.transform, lootBox.Health, lootBox);
        }

        public void Exit() => _waypointsFollower.Stop();
    }
}