using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class IdleState : IPlayerState, IState
    {
        public void Enter()
        {
            Debug.Log("Enter Idle State");
        }
    }
}