using System;
using Gameplay.Entities.Player.StateMachine.States.Core;
using UnityEngine;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class MoveState : IPayloadedPlayerState<MoveState.Payload>
    {
        public void Enter(Payload payload) { }

        public class Payload
        {
            public Vector3 Position;
            public Action OnComplete;
        }
    }
}