using System;
using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using DG.Tweening;
using Infrastructure.StateMachine.Main.States.Core;
using Platforms.DumpPlatform.Workers.StateMachine.States.Core;
using UnityEngine;

namespace Platforms.DumpPlatform.Workers.StateMachine.States
{
    public class IdleState : IState, IWorkerState, IExitable
    {
        private readonly DumpPlatformData _platformData;
        private readonly DumpPlatformPreferences _preferences;
        private readonly WorkerReferences _workerReferences;

        public IdleState(DumpPlatformData platformData, DumpPlatformPreferences preferences, WorkerReferences workerReferences)
        {
            _platformData = platformData;
            _preferences = preferences;
            _workerReferences = workerReferences;
        }

        private IDisposable _intervalSubscription;

        public void Enter()
        {
            HideTool();
        }

        public void Exit() { }

        private void HideTool()
        {
            Transform toolTransform = _workerReferences.ToolTransform;

            toolTransform
                .DOScale(Vector3.zero, _preferences.WorkerToolAnimationDuration)
                .SetEase(_preferences.WorkerToolAnimationCurve)
                .OnComplete(() => toolTransform.gameObject.SetActive(false))
                .Play();
        }
    }
}