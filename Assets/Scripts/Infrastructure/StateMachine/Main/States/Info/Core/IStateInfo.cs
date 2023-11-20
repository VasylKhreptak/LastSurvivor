using System;

namespace Infrastructure.StateMachine.Main.States.Info.Core
{
    public interface IStateInfo
    {
        Type StateType { get; }

        void Enter();

        void Tick();

        void Exit();
    }
}