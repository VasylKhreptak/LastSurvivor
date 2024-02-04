using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.LootBox
{
    public class LootBox : MonoBehaviour, IVisitable<BulletDamage>
    {
        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health)
        {
            _health = health;
        }

        public void Accept(BulletDamage visitor) => _health.TakeDamage(visitor.Value);
    }
}