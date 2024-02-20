using Gameplay.Entities.Collector.StateMachine.States.Core;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Collector
{
    [DisallowMultipleComponent]
    public class Collector : MonoBehaviour, IVisitable<ZombieDamage>
    {
        public IHealth Health { get; private set; }
        public IStateMachine<ICollectorState> StateMachine { get; private set; }

        [Inject]
        private void Constructor(IHealth health, IStateMachine<ICollectorState> stateMachine)
        {
            Health = health;
            StateMachine = stateMachine;
        }

        public void Accept(ZombieDamage zombieDamage) => Health.TakeDamage(zombieDamage.Value);
    }
}