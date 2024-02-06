using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.LootBox
{
    public class LootBox : MonoBehaviour, IVisitable<BulletDamage>, IVisitable<MeleeDamage>
    {
        public IHealth Health { get; private set; }

        [Inject]
        private void Constructor(IHealth health)
        {
            Health = health;
        }

        public void Accept(BulletDamage visitor) => Health.TakeDamage(visitor.Value);

        public void Accept(MeleeDamage visitor) => Health.TakeDamage(visitor.Value);
    }
}