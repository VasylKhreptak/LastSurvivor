using Gameplay.Weapons.Minigun.StateMachine.States.Core;
using UnityEngine;

namespace Gameplay.Weapons.Minigun.StateMachine.States
{
    public class FireState : IMinigunState
    {
        public void Enter()
        {
            Debug.Log("Fire!");
        }
        
    }
}