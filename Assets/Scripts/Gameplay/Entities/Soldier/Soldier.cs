using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Soldier
{
    public class Soldier : MonoBehaviour
    {
        public IStateMachine<ISoldierState> StateMachine { get; private set; }
        public SoldierAimer Aimer { get; private set; }
        public SoldierShooter Shooter { get; private set; }

        [Inject]
        private void Constructor(IStateMachine<ISoldierState> stateMachine, SoldierAimer aimer, SoldierShooter shooter)
        {
            StateMachine = stateMachine;
            Aimer = aimer;
            Shooter = shooter;
        }
    }
}