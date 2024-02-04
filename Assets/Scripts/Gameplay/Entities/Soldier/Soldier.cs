using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Soldier
{
    public class Soldier : MonoBehaviour, IVisitable<ZombieDamage>
    {
        public IStateMachine<ISoldierState> StateMachine { get; private set; }
        public SoldierAimer Aimer { get; private set; }
        public SoldierShooter Shooter { get; private set; }
        private IHealth _health;

        [Inject]
        private void Constructor(IStateMachine<ISoldierState> stateMachine, SoldierAimer aimer, SoldierShooter shooter, IHealth health)
        {
            StateMachine = stateMachine;
            Aimer = aimer;
            Shooter = shooter;
            _health = health;
        }

        public void Accept(ZombieDamage visitor) => _health.TakeDamage(visitor.Value);
    }
}