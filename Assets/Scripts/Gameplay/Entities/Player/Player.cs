using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class Player : MonoBehaviour, IVisitable<ZombieDamage>
    {
        public PlayerMapNavigator MapNavigator { get; private set; }
        private IHealth _health;

        [Inject]
        private void Constructor(PlayerMapNavigator mapNavigator, IHealth health)
        {
            MapNavigator = mapNavigator;
            _health = health;
        }

        public void Accept(ZombieDamage visitor) => _health.TakeDamage(visitor.Value);
    }
}