using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class Player : MonoBehaviour, IVisitable<ZombieDamage>
    {
        private IHealth _health;

        public IStateMachine<IPlayerState> StateMachine { get; private set; }

        [Inject]
        private void Constructor(IHealth health, IStateMachine<IPlayerState> stateMachine)
        {
            _health = health;
            StateMachine = stateMachine;
        }

        public void Accept(ZombieDamage visitor) => _health.TakeDamage(visitor.Value);
    }
}