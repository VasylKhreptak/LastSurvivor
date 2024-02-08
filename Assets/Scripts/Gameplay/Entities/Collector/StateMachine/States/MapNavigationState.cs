using System;
using Entities.AI;
using Gameplay.Entities.Collector.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;

namespace Gameplay.Entities.Collector.StateMachine.States
{
    public class MapNavigationState : ICollectorState, IState, IExitable
    {
        private readonly Transform _followPoint;
        private readonly AgentTransformFollower _agentTransformFollower;
        private readonly ClosestTriggerObserver<LootBox.LootBox> _closestLootBoxObserver;
        private readonly MeleeAttacker _meleeAttacker;

        public MapNavigationState(Transform followPoint, AgentTransformFollower agentTransformFollower,
            ClosestTriggerObserver<LootBox.LootBox> closestTriggerObserver, MeleeAttacker meleeAttacker)
        {
            _followPoint = followPoint;
            _agentTransformFollower = agentTransformFollower;
            _closestLootBoxObserver = closestTriggerObserver;
            _meleeAttacker = meleeAttacker;
        }

        private IDisposable _closestLootBoxSubscription;

        public void Enter() => StartObserving();

        public void Exit()
        {
            StopObserving();
            _meleeAttacker.Stop();
            _agentTransformFollower.Stop();
        }

        private void StartObserving()
        {
            StopObserving();
            _closestLootBoxSubscription = _closestLootBoxObserver.Trigger
                .Select(x => x?.Value)
                .Subscribe(OnClosestLootBoxChanged);
        }

        private void StopObserving() => _closestLootBoxSubscription?.Dispose();

        private void OnClosestLootBoxChanged(LootBox.LootBox lootBox)
        {
            if (lootBox == null)
            {
                _meleeAttacker.Stop();
                _agentTransformFollower.Follow(_followPoint);
                return;
            }

            _agentTransformFollower.Stop();
            _meleeAttacker.Start(lootBox.transform, lootBox.Health, lootBox);
        }
    }
}