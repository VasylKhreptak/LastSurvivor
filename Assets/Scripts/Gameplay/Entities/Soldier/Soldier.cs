using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Soldier
{
    public class Soldier : MonoBehaviour
    {
        [Inject]
        private void Constructor(IStateMachine<ISoldierState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public IStateMachine<ISoldierState> StateMachine { get; private set; }
    }
}