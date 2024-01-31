using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Soldier
{
    public class Soldier : MonoBehaviour
    {
        private IStateMachine<ISoldierState> _stateMachine;

        [Inject]
        private void Constructor(IStateMachine<ISoldierState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public IStateMachine<ISoldierState> StateMachine => _stateMachine;
    }
}