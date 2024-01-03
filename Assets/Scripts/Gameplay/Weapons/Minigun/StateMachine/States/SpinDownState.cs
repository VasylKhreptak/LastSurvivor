using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using UnityEngine;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class SpinDownState : IMinigunState
    {
        public void Enter()
        {
            Debug.Log("SpinDownState");
        }
    }
}