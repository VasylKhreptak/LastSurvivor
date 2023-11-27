using System;
using Data.Persistent.Platforms;
using Infrastructure.StateMachine.Main.States.Core;
using Platforms.DumpPlatform.Workers.StateMachine.States.Core;

namespace Platforms.DumpPlatform.Workers.StateMachine.States
{
    public class IdleState : IState, IWorkerState, IExitable
    {
        private readonly DumpPlatformData _platformData;

        public IdleState(DumpPlatformData platformData)
        {
            _platformData = platformData;
        }

        private IDisposable _intervalSubscription;

        public void Enter() { }

        public void Exit() { }
    }
}