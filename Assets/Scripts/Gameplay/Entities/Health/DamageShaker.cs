using Gameplay.Entities.Health.Core;
using Utilities.CameraUtilities.Shaker;

namespace Gameplay.Entities.Health
{
    public class DamageShaker : DamageObserver
    {
        private readonly ShakeLayer _shakeLayer;

        public DamageShaker(ShakeLayer shakeLayer, IHealth health) : base(health)
        {
            _shakeLayer = shakeLayer;
        }

        protected override void OnTookDamage(float damage) => _shakeLayer.Shake();
    }
}