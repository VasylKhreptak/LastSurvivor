using System;
using Data.Persistent.Platforms;
using Infrastructure.StateMachine.Main.Core;
using Main.Platforms.DumpPlatform.Workers.StateMachine.States.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Main.Platforms.DumpPlatform.Workers.StateMachine.States
{
    public class WorkerStateController : MonoBehaviour
    {
        private IStateMachine<IWorkerState> _stateMachine;
        private DumpPlatformData _platformData;

        [Inject]
        private void Constructor(IStateMachine<IWorkerState> stateMachine, DumpPlatformData platformData)
        {
            _stateMachine = stateMachine;
            _platformData = platformData;
        }

        private IDisposable _gridFullnessSubscription;

        #region MonoBehaviour

        private void OnEnable() => StartObserving();

        private void OnDisable()
        {
            StopObserving();
            _stateMachine.Exit();
        }

        #endregion

        private void StartObserving() => _gridFullnessSubscription = _platformData.GearsBank.IsFull.Subscribe(OnGridFullnessChanged);

        private void StopObserving() => _gridFullnessSubscription?.Dispose();

        private void OnGridFullnessChanged(bool isFull)
        {
            if (isFull)
                _stateMachine.Enter<IdleState>();
            else
                _stateMachine.Enter<WorkState>();
        }
    }
}