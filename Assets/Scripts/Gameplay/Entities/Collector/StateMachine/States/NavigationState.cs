using System;
using System.Collections.Generic;
using Entities.AI;
using Gameplay.Entities.Collector.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;

namespace Gameplay.Entities.Collector.StateMachine.States
{
    public class NavigationState : ICollectorState, IState, IExitable
    {
        private readonly Transform _followPoint;
        private readonly AgentTransformFollower _agentTransformFollower;
        private readonly ClosestTriggerObserver<LootBox.LootBox> _closestLootBoxObserver;
        private readonly MeleeAttacker _meleeAttacker;
        private readonly List<Collector> _collectors;
        private readonly Collector _collector;

        public NavigationState(Transform followPoint, AgentTransformFollower agentTransformFollower,
            ClosestTriggerObserver<LootBox.LootBox> closestTriggerObserver, MeleeAttacker meleeAttacker, List<Collector> collectors,
            Collector collector)
        {
            _followPoint = followPoint;
            _agentTransformFollower = agentTransformFollower;
            _closestLootBoxObserver = closestTriggerObserver;
            _meleeAttacker = meleeAttacker;
            _collectors = collectors;
            _collector = collector;
        }

        private IDisposable _closestLootBoxSubscription;

        public void Enter()
        {
            Debug.Log("NavigationState Enter");
            StartObserving();
        }

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
                _agentTransformFollower.Start(_followPoint);
                return;
            }

            _agentTransformFollower.Stop();
            _meleeAttacker.Start(lootBox.CollectPoints[_collectors.IndexOf(_collector) + 1].position, lootBox.transform,
                lootBox.Health, lootBox);
        }
    }
}