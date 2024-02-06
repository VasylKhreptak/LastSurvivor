using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using UnityEngine;
using Utilities.CameraUtilities.Shaker;
using Visitor;
using Zenject;

namespace Gameplay.Entities.LootBox
{
    public class LootBox : MonoBehaviour, IVisitable<BulletDamage>, IVisitable<MeleeDamage>
    {
        public IHealth Health { get; private set; }
        private ShakeLayer _shakeLayer;

        [Inject]
        private void Constructor(IHealth health, ShakeLayer shakeLayer)
        {
            Health = health;
            _shakeLayer = shakeLayer;
        }

        public void Accept(BulletDamage visitor)
        {
            Health.TakeDamage(visitor.Value);
            _shakeLayer.Shake();
        }

        public void Accept(MeleeDamage visitor)
        {
            Health.TakeDamage(visitor.Value);
            _shakeLayer.Shake();
        }
    }
}