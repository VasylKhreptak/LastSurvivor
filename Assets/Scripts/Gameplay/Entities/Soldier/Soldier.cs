using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Soldier
{
    [DisallowMultipleComponent]
    public class Soldier : MonoBehaviour, IVisitable<ZombieDamage>
    {
        public IStateMachine<ISoldierState> StateMachine { get; private set; }
        public IHealth Health { get; private set; }

        [Inject]
        private void Constructor(IStateMachine<ISoldierState> stateMachine, IHealth health)
        {
            StateMachine = stateMachine;
            Health = health;
        }

        public void Accept(ZombieDamage visitor) => Health.TakeDamage(visitor.Value);
    }
}