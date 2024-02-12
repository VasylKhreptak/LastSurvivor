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
        private IHealth _health;

        [Inject]
        private void Constructor(IStateMachine<ISoldierState> stateMachine, IHealth health)
        {
            StateMachine = stateMachine;
            _health = health;
        }

        public void Accept(ZombieDamage visitor) => _health.TakeDamage(visitor.Value);
    }
}