using System;
using Entities.AI;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Gameplay.Levels.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UniRx;
using Utilities.PhysicsUtilities.Trigger;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class NavigationState : IPlayerState, IState, IExitable
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly AgentWaypointsFollower _waypointsFollower;
        private readonly MeleeAttacker _meleeAttacker;
        private readonly ClosestTriggerObserver<LootBox.LootBox> _lootBoxObserver;

        public NavigationState(IStateMachine<ILevelState> levelStateMachine, AgentWaypointsFollower waypointsFollower,
            MeleeAttacker meleeAttacker, ClosestTriggerObserver<LootBox.LootBox> lootBoxObserver)
        {
            _levelStateMachine = levelStateMachine;
            _waypointsFollower = waypointsFollower;
            _meleeAttacker = meleeAttacker;
            _lootBoxObserver = lootBoxObserver;
        }

        private IDisposable _lootBoxObserverSubscription;

        public void Enter() => StartObserving();

        public void Exit()
        {
            StopObserving();
            _meleeAttacker.Stop();
            _waypointsFollower.Stop();
        }

        private void StartObserving()
        {
            StopObserving();
            _lootBoxObserverSubscription = _lootBoxObserver.Trigger.Select(x => x?.Value).Subscribe(OnClosestLootBoxChanged);
        }

        private void StopObserving() => _lootBoxObserverSubscription?.Dispose();

        private void OnClosestLootBoxChanged(LootBox.LootBox lootBox)
        {
            if (lootBox == null)
            {
                _meleeAttacker.Stop();
                _waypointsFollower.Start(() => _levelStateMachine.Enter<LevelCompletedState>());
                return;
            }

            _waypointsFollower.Stop();
            _meleeAttacker.Start(lootBox.CollectPoints[0].position, lootBox.transform, lootBox.Health, lootBox);
        }
    }
}