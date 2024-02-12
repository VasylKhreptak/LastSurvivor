using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    [DisallowMultipleComponent]
    public class Zombie : MonoBehaviour, IVisitable<BulletDamage>, IVisitable<ExplosionDamage>
    {
        private IStateMachine<IZombieState> _stateMachine;
        private IHealth _health;

        [Inject]
        private void Constructor(IStateMachine<IZombieState> stateMachine, IHealth health)
        {
            _stateMachine = stateMachine;
            _health = health;
        }

        public IStateMachine<IZombieState> StateMachine => _stateMachine;

        public void Accept(BulletDamage visitor) => _health.TakeDamage(visitor.Value);

        public void Accept(ExplosionDamage visitor) => _health.TakeDamage(visitor.Value);
    }
}